using API.AutoMapper;
using Core.AutoMapper;
using Core.BugService;
using Core.DbService;
using Core.Other;
using Core.UserService;
using Infrastructure;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            string dbAccessCreds = Environment.GetEnvironmentVariable("DbAccessCreds");

            builder.Services.AddDbContext<TrackerDbContext>(opt =>
            {
                opt.UseSqlServer(string.Format(builder.Configuration.GetConnectionString("BugTracker"), dbAccessCreds));
            })
                .AddIdentity<BugUser, IdentityRole>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequiredUniqueChars = 0;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<TrackerDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<BugUser>>()
                .AddUserManager<UserManager<BugUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>();

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/users/login";
                opt.LogoutPath = "/users/logout";
                opt.ExpireTimeSpan = TimeSpan.FromDays(1);
                opt.SlidingExpiration = true;
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;

                opt.Events.OnRedirectToLogin = c =>
                {
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    return Task.CompletedTask;
                };
            });

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy(AuthorizePolicy.Admin, p => p.RequireRole(UserRoles.Admin));
                opt.AddPolicy(AuthorizePolicy.Manager, p => p.RequireRole(UserRoles.Manager));
                opt.AddPolicy(AuthorizePolicy.User, p => p.RequireRole(UserRoles.User));
            });

            builder.Services.AddScoped<ITrackerDbService, TrackerDbService>();
            builder.Services.AddScoped<IBugService, BugService>();
            builder.Services.AddScoped<IUserService<BugUser>, UserService<BugUser>>()
                .AddHttpContextAccessor();

            builder.Services.AddAutoMapper(opt =>
            {
                opt.AddProfile(typeof(BugProfile));
                opt.AddProfile(typeof(BugUserProfile));
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await Initialize(builder, app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }

        private static async Task Initialize(WebApplicationBuilder builder, WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
                var userManager = (UserManager<BugUser>)scope.ServiceProvider.GetRequiredService(typeof(UserManager<BugUser>));

                var initializer = new Initializer(roleManager, userManager, builder.Configuration);

                await initializer.InitializeRoles();
            }
        }
    }
}

using API.AutoMapper;
using Core.BugService;
using Core.DbService;
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
        public static void Main(string[] args)
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
                .AddUserManager<UserManager<BugUser>>();

            builder.Services.ConfigureApplicationCookie(opt =>
                {
                    opt.LoginPath = "/users/login";
                    opt.LogoutPath = "/users";
                    opt.ExpireTimeSpan = TimeSpan.FromDays(1);
                    opt.SlidingExpiration = true;
                    opt.Cookie.HttpOnly = true;
                    opt.Cookie.IsEssential = true;
                });

            builder.Services.AddScoped<ITrackerDbService, TrackerDbService>();
            builder.Services.AddScoped<IBugService, BugService>();
            builder.Services.AddScoped<IUserService<BugUser>, UserService<BugUser>>()
                .AddHttpContextAccessor();

            builder.Services.AddAutoMapper(opt =>
                {
                    opt.AddProfile(typeof(BugProfile));
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
    }
}

using API.AutoMapper;
using Core.AutoMapper;
using Core.Entities.UserEntity;
using Core.EntitiesQueryUtilities.Bugs;
using Core.EntitiesQueryUtilities.Comments;
using Core.EntitiesQueryUtilities.QueryBuilders;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;
using Core.EntitiesQueryUtilities.QueryParameters.Comments;
using Core.EntitiesQueryUtilities.QueryParameters.Replies;
using Core.EntitiesQueryUtilities.Replies;
using Core.Other;
using Core.Repositories;
using Core.Services.BugService;
using Core.Services.CommentService;
using Core.Services.ReplyService;
using Core.Services.UserService;
using Core.Utilities;
using Core.Utilities.JsonConverters;
using DotNetEnv;
using DotNetEnv.Extensions;
using Infrastructure;
using Infrastructure.QueryBuilders;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            if (builder.Environment.IsDevelopment())
            {
                var envVars = Env.Load("../.env.development").ToDotEnvDictionary();

                EnvVariableService.LoadEnvironmentVariables(envVars);

                string dbConnStringDev = builder.Configuration["ConnectionStrings:BugTracker"];

                EnvVariableService.SetConnectionString(dbConnStringDev);
            }

            string dbConnString = EnvVariableService.GetConnectionString();

            builder.Services.AddDbContext<TrackerDbContext>(opt =>
            {
                opt.UseSqlServer(dbConnString);
            })
                .AddIdentity<BugUser, IdentityRole>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.SignIn.RequireConfirmedAccount = false;
                    opt.SignIn.RequireConfirmedEmail = false;

                    if (builder.Environment.IsDevelopment())
                    {
                        opt.Password.RequireDigit = false;
                        opt.Password.RequiredUniqueChars = 0;
                        opt.Password.RequireNonAlphanumeric = false;
                        opt.Password.RequireUppercase = false;
                        opt.Password.RequireLowercase = false;
                    }
                })
                .AddEntityFrameworkStores<TrackerDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<BugUser>>()
                .AddUserManager<UserManager<BugUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                string jwtSecretKey = EnvVariableService.GetJwtSecretKey();

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = EnvVariableService.GetJwtIssuer(),
                    ValidAudience = EnvVariableService.GetJwtAudience(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name,
                };

                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                        {
                            string? token = authHeader.FirstOrDefault()?.Replace("Bearer ", "");
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        var errorMessage = new { error = "Authentication failed." };

                        return context.Response.WriteAsync(JsonSerializer.Serialize(errorMessage));
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization(opt =>
            {
                string[] rolesForUserPolicy = new[] { UserRoles.User, UserRoles.Manager, UserRoles.Admin };

                var userPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole(rolesForUserPolicy)
                    .Build();

                string[] rolesForManagerPolicy = new[] { UserRoles.Manager, UserRoles.Admin };

                var managerPolicy = new AuthorizationPolicyBuilder()
                    .Combine(userPolicy)
                    .RequireRole(rolesForManagerPolicy)
                    .Build();

                string rolesForAdminPolicy = UserRoles.Admin;

                var adminPolicy = new AuthorizationPolicyBuilder()
                    .Combine(managerPolicy)
                    .RequireRole(rolesForAdminPolicy)
                    .Build();

                opt.AddPolicy(AuthorizePolicy.UserAccess, userPolicy);
                opt.AddPolicy(AuthorizePolicy.ManagerAccess, managerPolicy);
                opt.AddPolicy(AuthorizePolicy.AdminAccess, adminPolicy);
            });

            builder.Services.AddScoped<IBugService, BugService>();
            builder.Services.AddScoped<IBugRepository, BugRepository>();
            builder.Services.AddScoped<IBugQueryableBuilder, BugQueryableBuilder>();
            builder.Services.AddScoped<IBugQueryParametersFactory, BugQueryParametersFactory>();
            builder.Services.AddScoped<IBugFilterFactory, BugFilterFactory>();
            builder.Services.AddScoped<IBugSortingOptionsFactory, BugSortingOptionsFactory>();

            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ICommentQueryableBuilder, CommentQueryableBuilder>();
            builder.Services.AddScoped<ICommentQueryParametersFactory, CommentQueryParametersFactory>();
            builder.Services.AddScoped<ICommentFilterFactory, CommentFilterFactory>();
            builder.Services.AddScoped<ICommentSortingOptionsFactory, CommentSortingOptionsFactory>();

            builder.Services.AddScoped<IReplyService, ReplyService>();
            builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
            builder.Services.AddScoped<IReplyQueryableBuilder, ReplyQueryableBuilder>();
            builder.Services.AddScoped<IReplyQueryParametersFactory, ReplyQueryParametersFactory>();
            builder.Services.AddScoped<IReplyFilterFactory, ReplyFilterFactory>();
            builder.Services.AddScoped<IReplySortingOptionFactory, ReplySortingOptionsFactory>();

            builder.Services.AddScoped<IUserService<BugUser>, UserService<BugUser>>()
                .AddHttpContextAccessor();

            builder.Services.AddAutoMapper(opt =>
            {
                opt.AddProfile(typeof(BugProfile));
                opt.AddProfile(typeof(BugUserProfile));
                opt.AddProfile(typeof(CommentProfile));
                opt.AddProfile(typeof(ReplyProfile));
                opt.AddProfile(typeof(QueryProfile));
            });

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid Bearer token in the text input below.",
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddCors(opt =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    opt.AddPolicy("Any", p =>
                    {
                        p.AllowAnyOrigin();
                        p.AllowAnyHeader();
                        p.AllowAnyMethod();
                    });
                }
                else
                {
                    opt.AddPolicy("ReactFrontEnd", p =>
                    {
                        p.WithOrigins(EnvVariableService.GetCorsOriginsUrl());
                        p.AllowAnyHeader();
                        p.AllowAnyMethod();
                    });
                }
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            string corsPolicy = string.Empty;

            if (builder.Environment.IsDevelopment())
            {
                corsPolicy = "Any";
            }
            else
            {
                corsPolicy = "ReactFrontEnd";
            }

            app.UseCors(corsPolicy);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await Initialize(builder, app);

            app.Run();
        }

        private static async Task Initialize(WebApplicationBuilder builder, WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
                var userManager = (UserManager<BugUser>)scope.ServiceProvider.GetRequiredService(typeof(UserManager<BugUser>));
                var dbContext = (TrackerDbContext)scope.ServiceProvider.GetRequiredService(typeof(TrackerDbContext));

                var initializer = new Initializer(roleManager, userManager, builder.Configuration, dbContext);

                await initializer.Initialize();
            }
        }
    }
}

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
using Core.Services.SearchesService;
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

            if (builder.Environment.IsDevelopment())
            {
                var envVars = Env.Load("../.env.development").ToDotEnvDictionary();

                EnvVariableService.LoadEnvironmentVariables(envVars);

                string dbConnStringDev = builder.Configuration[$"ConnectionStrings:BugTracker"]!;

                EnvVariableService.SetConnectionString(dbConnStringDev);
            }

            // Add services to the container.
            builder.Services.AddDbWithIdentity(builder.Environment);

            builder.Services.AddAppAuthentication();
            builder.Services.AddAppAuthorization();

            builder.Services.AddBugServices()
                .AddCommentServices()
                .AddReplyServices()
                .AddMiscellaneousServices();

            builder.Services.AddScoped<IUserService<BugUser>, UserService<BugUser>>()
                .AddHttpContextAccessor();

            builder.Services.AddAutoMapperProfiles();

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddSignalR();

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
                            p.WithOrigins("http://localhost:5173");
                            p.AllowAnyHeader();
                            p.AllowAnyMethod();
                            p.AllowCredentials();
                        });
                    }
                    else
                    {
                        opt.AddPolicy("ReactFrontEnd", p =>
                        {
                            p.WithOrigins(EnvVariableService.GetCorsOriginsUrl());
                            p.AllowAnyHeader();
                            p.AllowAnyMethod();
                            p.AllowCredentials();
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

            app.UseRouting();
            app.UseCors(corsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            await Initialize(builder, app);

            app.MapControllers();
            app.MapHub<NotificationHub>("/notifs", options =>
            {
                options.CloseOnAuthenticationExpiration = true;
            });

            app.Run();
        }

        private static async Task Initialize(WebApplicationBuilder builder, WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
                var userManager = (UserManager<BugUser>)scope.ServiceProvider.GetRequiredService(typeof(UserManager<BugUser>));
                var dbContext = (TrackerDbContext)scope.ServiceProvider.GetRequiredService(typeof(TrackerDbContext));
                var searchesService = (SearchesService)scope.ServiceProvider.GetRequiredService(typeof(ISearchesService));

                var initializer = new Initializer(roleManager, userManager, builder.Configuration, dbContext, searchesService);

                await initializer.Initialize();
            }
        }
    }
}

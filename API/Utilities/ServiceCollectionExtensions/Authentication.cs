using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Core.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    else if (context.Request.Query.TryGetValue("access_token", out var accessToken))
                    {
                        context.Token = accessToken;
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

        return services;
    }
}

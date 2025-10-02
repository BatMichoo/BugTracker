using Core.Other;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
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

        return services;
    }
}

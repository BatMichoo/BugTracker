using Core.Entities.UserEntity;
using Core.Utilities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddDbWithIdentity(this IServiceCollection services, IWebHostEnvironment environment)
    {
        string dbConnString = EnvVariableService.GetConnectionString();

        services.AddDbContext<TrackerDbContext>(opt =>
        {
            opt.UseSqlServer(dbConnString);
        });

        services.AddIdentity<BugUser, IdentityRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedAccount = false;
            opt.SignIn.RequireConfirmedEmail = false;

            if (environment.IsDevelopment())
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

        return services;
    }
}

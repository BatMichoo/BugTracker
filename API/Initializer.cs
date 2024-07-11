using Infrastructure.Models.User;
using Microsoft.AspNetCore.Identity;

namespace API
{
    public class Initializer
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<BugUser> userManager;
        private readonly IConfiguration Config;

        public Initializer(RoleManager<IdentityRole> roleManager, UserManager<BugUser> userManager, IConfiguration config)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            Config = config;
        }

        public async Task InitializeRoles()
        {
            var roles = Config.GetSection("Roles:UserRoles").Get<string[]>();

            foreach (var role in roles) 
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = Config["RootAdmin:Email"];
            string adminUserName = Config["RootAdmin:UserName"];
            var rootUser = await userManager.FindByEmailAsync(adminEmail);

            if (rootUser == null)
            {
                var user = new BugUser()
                {
                    Email = adminEmail,
                    UserName = adminUserName,
                    Name = adminUserName
                };

                var result = await userManager.CreateAsync(user, Config["RootAdmin:Password"]);

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }
    }
}

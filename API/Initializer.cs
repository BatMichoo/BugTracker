using Core.Entities.UserEntity;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Initializer
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<BugUser> userManager;
        private readonly IConfiguration Config;
        private readonly TrackerDbContext _dbContext;

        public Initializer(RoleManager<IdentityRole> roleManager, UserManager<BugUser> userManager, IConfiguration config, TrackerDbContext dbContext)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            Config = config;
            _dbContext = dbContext;
        }

        public async Task Initialize()
        {
            await InitializeDatabase();
            await InitializeRoles();
        }

        private async Task InitializeDatabase(int retries = 0)
        {
            bool canConnetct = await _dbContext.Database.CanConnectAsync();

            if (canConnetct)
            {
                string connString = _dbContext.Database.GetConnectionString();
                Console.WriteLine($"====> Conn String is ${connString}");

                try
                {
                    await _dbContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (retries < 3)
            {
                retries++;
                await InitializeDatabase(retries);
            }
            else
            {
                throw new Exception("Couldn't establish database");
            }
        }

        private async Task InitializeRoles()
        {
            var roles = Config.GetSection("Roles:UserRoles").Get<string[]>()!;

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = Config["RootAdmin:Email"]!;
            string adminUserName = Config["RootAdmin:UserName"]!;
            var rootUser = await userManager.FindByEmailAsync(adminEmail);

            if (rootUser == null)
            {
                var user = new BugUser()
                {
                    Email = adminEmail,
                    UserName = adminUserName,
                    Name = adminUserName
                };

                var result = await userManager.CreateAsync(user, Config["RootAdmin:Password"]!);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}

using Core.Entities.CustomRole;
using Core.Entities.UserEntity;
using Core.Services.SearchesService;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Initializer
    {
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly UserManager<BugUser> _userManager;
        private readonly IConfiguration Config;
        private readonly TrackerDbContext _dbContext;
        private readonly ISearchesService _searchesService;

        public Initializer(RoleManager<CustomRole> roleManager, UserManager<BugUser> userManager, IConfiguration config, TrackerDbContext dbContext, ISearchesService searchesService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            Config = config;
            _dbContext = dbContext;
            _searchesService = searchesService;
        }

        public async Task Initialize()
        {
            await InitializeDatabase();
            await InitializeRoles();
        }

        private async Task InitializeDatabase()
        {
            try
            {
                await _dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task InitializeRoles()
        {
            var roles = Config.GetSection("Roles:UserRoles").Get<string[]>()!;

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new CustomRole(role, isDeletable: false));
                }
            }

            string adminEmail = Config["RootAdmin:Email"]!;
            string adminUserName = Config["RootAdmin:UserName"]!;
            var rootUser = await _userManager.FindByEmailAsync(adminEmail);

            if (rootUser == null)
            {
                var user = new BugUser()
                {
                    Email = adminEmail,
                    UserName = adminUserName,
                    Name = adminUserName
                };

                var result = await _userManager.CreateAsync(user, Config["RootAdmin:Password"]!);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _searchesService.CreateDefaultSavedSearches(user.Id);
                }
            }
        }
    }
}

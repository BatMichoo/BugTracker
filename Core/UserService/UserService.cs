using AutoMapper;
using Core.DTOs.User;
using Core.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.UserService
{
    public class UserService<T> : IUserService<T> where T : IdentityUser
    {
        private readonly UserManager<T> userManager;
        private readonly SignInManager<T> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly ClaimsPrincipal claimsPrincipal;

        public UserService(UserManager<T> userManager, SignInManager<T> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
            this.roleManager = roleManager;
        }

        public string RetrieveUserId()
        {
            return userManager.GetUserId(claimsPrincipal);
        }

        public async Task<T> RegisterNewUserWithPassword(RegisterUserModel newUser)
        {
            var toBeCreated = mapper.Map<T>(newUser);

            var result = await userManager.CreateAsync(toBeCreated, newUser.Password);            

            if (result.Succeeded)
            {
                var roleSucceeded = await AddRolesToUser(toBeCreated, new List<string>() { UserRoles.User });

                if (roleSucceeded)
                {
                    return toBeCreated;
                }

                throw new ArgumentException("Roles could not be added.");
            }

            throw new ArgumentException(string.Join(Environment.NewLine, result.Errors));
        }

        public async Task<T> RetrieveUser()
        {
            var user = await userManager.GetUserAsync(claimsPrincipal);

            return user;
        }

        public async Task<T> RetrieveUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user;
        }       

        public async Task<bool> SignInUserWithPassword(T user, string password)
        {
            var result = await signInManager.PasswordSignInAsync(user, password, false, false);

            return result.Succeeded;
        }

        public async Task SignOut()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> AddRolesToUser(T user, IEnumerable<string> roles)
        {
            var result = await userManager.AddToRolesAsync(user, roles);

            return result.Succeeded;
        }

        public async Task<List<UserViewModel>> RetrieveUserList()
        {
            var users = await userManager.Users
                .Where(u => u.UserName != "Admin")
                .AsNoTracking()
                .ToListAsync();

            if (users.Any())
            {
                return mapper.Map<List<UserViewModel>>(users);
            }

            return new List<UserViewModel>();
        }

        public async Task<List<string>> GetRoles()
        {
            var roles = await roleManager.Roles
                .AsNoTracking()
                .Where(r => r.Name != UserRoles.Admin)
                .Select(r => r.Name)
                .ToListAsync();

            if (roles.Any())
            {
                return roles;
            }

            return new List<string>();
        }

        public async Task<T> RetrieveUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            return user;
        }
    }
}

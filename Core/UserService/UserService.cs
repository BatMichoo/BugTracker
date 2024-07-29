using AutoMapper;
using Core.DTOs.Users;
using Core.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.UserService
{
    public class UserService<T> : IUserService<T> where T : IdentityUser
    {
        private readonly UserManager<T> _userManager;
        private readonly SignInManager<T> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public UserService(UserManager<T> userManager, SignInManager<T> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
            _roleManager = roleManager;
        }

        public string RetrieveUserId()
        {
            return _userManager.GetUserId(_claimsPrincipal);
        }

        public async Task<T> RegisterNewUserWithPassword(RegisterUserModel newUser)
        {
            var toBeCreated = _mapper.Map<T>(newUser);

            var result = await _userManager.CreateAsync(toBeCreated, newUser.Password);            

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
            var user = await _userManager.GetUserAsync(_claimsPrincipal);

            return user;
        }

        public async Task<T> RetrieveUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }       

        public async Task<bool> SignInUserWithPassword(T user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            return result.Succeeded;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> AddRolesToUser(T user, IEnumerable<string> roles)
        {
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result.Succeeded;
        }

        public async Task<List<UserViewModel>> RetrieveUserList()
        {
            var users = await _userManager.Users
                .Where(u => u.UserName != "Admin")
                .AsNoTracking()
                .ToListAsync();

            if (users.Any())
            {
                return _mapper.Map<List<UserViewModel>>(users);
            }

            return new List<UserViewModel>();
        }

        public async Task<List<string>> GetRoles()
        {
            var roles = await _roleManager.Roles
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
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }
    }
}

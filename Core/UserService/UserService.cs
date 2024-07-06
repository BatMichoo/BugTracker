using AutoMapper;
using Core.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.UserService
{
    public class UserService<T> : IUserService<T> where T : IdentityUser
    {
        private readonly UserManager<T> userManager;
        private readonly SignInManager<T> signInManager;
        private readonly IMapper mapper;
        private readonly ClaimsPrincipal claimsPrincipal;

        public UserService(UserManager<T> userManager, SignInManager<T> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.claimsPrincipal = httpContextAccessor.HttpContext.User;
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
                return toBeCreated;
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
            T user = await userManager.FindByEmailAsync(email);

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
    }
}

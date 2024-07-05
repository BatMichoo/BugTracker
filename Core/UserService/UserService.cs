using Infrastructure.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<BugUser> userManager;
        private readonly SignInManager<BugUser> signInManager;

        public UserService(UserManager<BugUser> userManager, SignInManager<BugUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public string GetCurrentUserId(ClaimsPrincipal claimsPrincipal)
        {
            return userManager.GetUserId(claimsPrincipal);
        }
    }
}

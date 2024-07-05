using System.Security.Claims;

namespace Core.UserService
{
    public interface IUserService
    {
        string GetCurrentUserId(ClaimsPrincipal claimsPrincipal);
    }
}

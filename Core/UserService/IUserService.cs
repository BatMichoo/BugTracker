using Core.DTOs.User;
using System.Security.Claims;

namespace Core.UserService
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterUserModel user, ClaimsIdentity identity);
    }
}

using Core.DTOs.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.UserService
{
    public interface IUserService<T> where T : IdentityUser
    {
        string RetrieveUserId();
        Task<T> RetrieveUserByEmail(string email);
        Task<T> RetrieveUser();
        Task<T> RegisterNewUserWithPassword(RegisterUserModel newUser);
        Task<bool> SignInUserWithPassword(T user, string password);
        Task SignOut();
    }
}

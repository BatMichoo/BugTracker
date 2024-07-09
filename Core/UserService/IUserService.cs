using Core.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Core.UserService
{
    public interface IUserService<T> where T : IdentityUser
    {
        string RetrieveUserId();
        Task<bool> AddRolesToUser(T user, IEnumerable<string> roles);
        Task<T> RetrieveUserByEmail(string email);
        Task<T> RetrieveUser();
        Task<List<UserViewModel>> RetrieveUserList();
        Task<T> RegisterNewUserWithPassword(RegisterUserModel newUser);
        Task<bool> SignInUserWithPassword(T user, string password);
        Task SignOut();
    }
}

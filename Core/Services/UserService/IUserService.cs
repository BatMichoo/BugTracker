using Core.DTOs.Users;
using Core.Entities.CustomRole;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.UserService
{
    public interface IUserService<T>
        where T : IdentityUser
    {
        string RetrieveUserId();
        Task<bool> AddRolesToUser(T user, IEnumerable<string> roles);
        Task<bool> RemoveRolesFromUser(T user, IEnumerable<string> roles);
        Task<T> RetrieveUserByEmail(string email);
        Task<T> RetrieveUserById(string id);
        Task<T> RetrieveUser();
        Task<List<UserViewModel>> RetrieveUserList();
        Task<T> RegisterNewUserWithPassword(RegisterUserModel newUser);
        Task<bool> UpdateUser(T user);
        Task<bool> DeleteUser(T user);
        Task<bool> SignInUserWithPassword(T user, string password);
        Task<bool> ChangePassword(T user, string oldPassword, string newPassword);
        Task SignOut();

        Task<IEnumerable<object>> GetAllUserRoles();
        Task<LoginResponseModel> GenerateLoginResponse(T user);
        Task<CustomRole?> GetRoleByName(string name);
        Task<bool> UpdateRole(CustomRole role);
        Task<bool> CreateRole(CustomRole newRole);
        Task<bool> DeleteRole(CustomRole newRole);
        Task<List<CustomRole>> GetUserRoles(string userId);
    }
}

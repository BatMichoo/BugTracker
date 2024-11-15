using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Users
{
    public class LoginUserModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

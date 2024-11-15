using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Users
{
    public class RegisterUserModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Range(18, 70)]
        public int? Age { get; set; }
    }
}

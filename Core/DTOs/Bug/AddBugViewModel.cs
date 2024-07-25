using Infrastructure.Models.Bug;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Bug
{
    public class AddBugViewModel
    {
        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public string Priority { get; set; } = null!;

        [Required]
        [MaxLength(BugValidation.MaxLength)]
        public string Description { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Bug
{
    public class AddBugModel
    {
        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public string Priority { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        public string CreatorId { get; set; } = null!;
    }
}

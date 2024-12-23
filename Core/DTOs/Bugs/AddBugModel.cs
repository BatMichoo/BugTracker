using Core.Models.Bugs.BugEnums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Bugs
{
    public class AddBugModel
    {
        [Required]
        public BugStatus Status { get; set; } = BugStatus.InProgress;

        [Required]
        public BugPriority Priority { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(3000)]
        public string Description { get; set; } = null!;

        [Required]
        public string CreatorId { get; set; } = null!;

        public string? AssignedTo { get; set; }
    }
}

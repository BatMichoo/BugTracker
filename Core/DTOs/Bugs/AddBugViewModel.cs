using Core.Models.Bugs.BugEnums;
using Infrastructure.Models.BugEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Bugs
{
    public class AddBugViewModel
    {
        [Required]
        public BugStatus Status { get; set; }

        [Required]
        public BugPriority Priority { get; set; }

        [Required]
        [MaxLength(BugValidation.MaxLength)]
        public string Description { get; set; } = null!;
    }
}

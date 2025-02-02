using Core.Models.Bugs.BugEnums;
using Infrastructure.Models.BugEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Bugs
{
    public class EditBugViewModel
    {
        public int Id { get; set; }
        public BugStatus? Status { get; set; }
        public BugPriority? Priority { get; set; }

        [MaxLength(BugValidation.DescMaxLength)]
        public string? Description { get; set; }
        public string? AssigneeId { get; set; } 

        public bool Validate()
        {
            bool hasStatus = Status != null;
            bool hasPriority = Priority != null;
            bool hasDescription = !string.IsNullOrEmpty(Description);
            bool isAssigned = !string.IsNullOrEmpty(AssigneeId);

            return hasStatus || hasPriority || hasDescription || isAssigned;
        }
    }
}

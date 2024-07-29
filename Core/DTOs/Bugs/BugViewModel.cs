using Core.DTOs.Comments;
using Core.Models.Bugs.BugEnums;

namespace Core.DTOs.Bugs
{
    public class BugViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public BugStatus Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public BugPriority Priority { get; set; }
        public string Description { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string LastUpdatedBy { get; set; } = null!;
        public bool IsAssigned => !string.IsNullOrEmpty(AssignedTo);
        public string? AssignedTo { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}

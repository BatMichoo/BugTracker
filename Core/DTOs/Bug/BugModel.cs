using Core.Models.Bug.BugEnums;

namespace Core.DTOs.Bug
{
    public class BugModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public BugStatus Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public BugPriority Priority { get; set; }
        public string Description { get; set; } = null!;
        public string CreatorId { get; set; } = null!;
        public string Creator { get; set; } = null!;
        public bool IsAssigned => AssigneeId != null;
        public string? AssigneeId { get; set; }
        public string? Assignee { get; set; }
        public string LastUpdatedById { get; set; } = null!;
        public string LastUpdatedBy { get; set; } = null!;
    }
}

using Core.Models.Bug.BugEnums;

namespace Core.DTOs.Bug
{
    public class AddBugModel
    {
        public DateTime CreatedOn { get; set; }
        public BugStatus Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public BugPriority Priority { get; set; }
        public string Description { get; set; } = null!;
        public string? CreatorId { get; set; }
    }
}

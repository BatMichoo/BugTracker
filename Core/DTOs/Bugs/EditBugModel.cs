using Core.Models.Bugs.BugEnums;

namespace Core.DTOs.Bugs
{
    public class EditBugModel
    {
        public int Id { get; set; }
        public BugStatus? Status { get; set; }
        public BugPriority? Priority { get; set; }
        public string? Description { get; set; }
        public string? AssigneeId { get; set; }
        public string? LastUpdatedById { get; set; }
    }
}

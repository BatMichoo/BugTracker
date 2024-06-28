using Infrastructure.Models.Bug;

namespace Core.DTOs.Bug
{
    public class BugModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public BugStatus Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public BugPriority Priority { get; set; }
        public string Description { get; set; }
    }
}

using Core.Entities.CommentEntity;
using Core.Entities.UserEntity;

namespace Core.Entities.BugEntity
{
    public class Bug : BaseModel
    {
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int Priority { get; set; }

        public string LastUpdatedById { get; set; } = null!;
        public BugUser LastUpdatedBy { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string CreatorId { get; set; } = null!;
        public BugUser Creator { get; set; } = null!;

        public string? AssigneeId { get; set; }
        public BugUser? Assignee { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

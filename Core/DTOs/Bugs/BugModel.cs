using Core.DTOs.Comments;
using Core.DTOs.Users;
using Core.Entities;
using Core.Models.Bugs.BugEnums;

namespace Core.DTOs.Bugs
{
    public class BugModel : BaseModel
    {
        public DateTime CreatedOn { get; set; }
        public BugStatus Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public BugPriority Priority { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public UserModel CreatedBy { get; set; } = null!;
        public UserModel LastUpdatedBy { get; set; } = null!;
        public UserModel? AssignedTo { get; set; }
        public bool IsAssigned => AssignedTo != null;
        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}

using Core.DTOs.Comments;
using Core.DTOs.Users;

namespace Core.DTOs.Bugs
{
    public class BugViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int Priority { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public UserViewModel CreatedBy { get; set; } = null!;
        public UserViewModel LastUpdatedBy { get; set; } = null!;
        public bool IsAssigned => AssignedTo != null;
        public UserViewModel AssignedTo { get; set; } = null!;
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}

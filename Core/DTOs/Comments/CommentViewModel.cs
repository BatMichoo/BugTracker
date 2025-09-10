using Core.DTOs.Replies;

namespace Core.DTOs.Comments
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int Likes { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int BugId { get; set; }
        public string AuthorName { get; set; } = null!;
        public List<ReplyViewModel> Replies { get; set;} = [];
    }
}

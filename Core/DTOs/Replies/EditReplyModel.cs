using Core.Entities;

namespace Core.DTOs.Replies
{
    public class EditReplyModel : BaseModel
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
    }
}

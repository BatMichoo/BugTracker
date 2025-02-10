using Core.Entities.CommentEntity;
using Core.Entities.UserEntity;

namespace Core.Entities.ReplyEntity
{
    public class Reply : BaseModel
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string AuthorId { get; set; } = null!;
        public BugUser Author { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}

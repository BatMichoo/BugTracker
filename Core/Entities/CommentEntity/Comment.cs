using Core.Entities.BugEntity;
using Core.Entities.ReplyEntity;
using Core.Entities.UserEntity;

namespace Core.Entities.CommentEntity
{
    public class Comment : BaseModel
    {
        public string Content { get; set; } = null!;

        public int Likes { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public int BugId { get; set; }
        public Bug Bug { get; set; } = null!;

        public string AuthorId { get; set; } = null!;
        public BugUser Author { get; set; } = null!;

        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}

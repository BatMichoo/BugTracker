using Infrastructure.Models.UserEntity;

namespace Core.DTOs.Comments
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int Likes { get; set; }
        public DateTime PostedOn { get; set; }
        public int BugId { get; set; }
        public string AuthorId { get; set; } = null!;
        public BugUser Author { get; set; } = null!;
    }
}

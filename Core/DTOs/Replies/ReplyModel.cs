namespace Core.DTOs.Replies
{
    public class ReplyModel
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string Content { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}

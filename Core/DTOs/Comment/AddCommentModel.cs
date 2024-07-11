namespace Core.DTOs.Comment
{
    public class AddCommentModel
    {
        public string Content { get; set; } = null!;
        public int BugId { get; set; }
        public string? AuthorId { get; set; } 
    }
}

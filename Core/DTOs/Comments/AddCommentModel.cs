namespace Core.DTOs.Comments
{
    public class AddCommentModel
    {
        public string Content { get; set; } = null!;
        public int BugId { get; set; }
        public string? AuthorId { get; set; } 
    }
}

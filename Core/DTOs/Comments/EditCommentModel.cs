namespace Core.DTOs.Comments
{
    public class EditCommentModel : BaseModel
    {
        public string Content { get; set; } = null!;
        public DateTime LastUpdatedOn { get; set; }
    }
}

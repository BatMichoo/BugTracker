namespace Infrastructure.Models.BugComment
{
    public class BugComment
    {
        public int BugId { get; set; }
        public Bug.Bug Bug { get; set; }

        public int CommentId { get; set; }
        public Comment.Comment Comment { get; set; }
    }
}

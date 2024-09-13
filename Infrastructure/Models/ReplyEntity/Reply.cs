using Infrastructure.Models.CommentEntity;
using Infrastructure.Models.UserEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.ReplyEntity
{
    public class Reply : BaseEntity
    {
        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }
        public Comment Comment { get; set; } = null!;

        [Required]
        [StringLength(ReplyValidation.MaxContentLenght)]
        public string Content { get; set; } = null!;

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = null!;
        public BugUser Author { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}

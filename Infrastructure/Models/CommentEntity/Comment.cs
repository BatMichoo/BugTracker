using Infrastructure.Models.BugEntity;
using Infrastructure.Models.ReplyEntity;
using Infrastructure.Models.UserEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.CommentEntity
{
    public class Comment : BaseEntity
    {
        [Required]
        [MaxLength(CommentValidation.MaxLength)]
        public string Content { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Likes { get; set; }
        public DateTime PostedOn { get; set; }

        [ForeignKey(nameof(Bug))]
        public int BugId { get; set; }
        public Bug Bug { get; set; } = null!;

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = null!;
        public BugUser Author { get; set; } = null!;

        public List<Reply> Replies { get; set; } = new List<Reply>(); 
    }
}

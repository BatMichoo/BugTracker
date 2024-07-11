using Infrastructure.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.Comment
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = null!;
        public int Likes { get; set; }
        public DateTime PostedOn { get; set; }

        [ForeignKey(nameof(Bug))]
        public int BugId { get; set; }
        public Bug.Bug Bug { get; set; } = null!;

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
        public BugUser Author { get; set; } = null!;
    }
}

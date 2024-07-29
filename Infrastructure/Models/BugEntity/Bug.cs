using Infrastructure.Models.CommentEntity;
using Infrastructure.Models.UserEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.BugEntity
{
    public class Bug : BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int Priority { get; set; }

        [ForeignKey(nameof(LastUpdatedBy))]
        public string LastUpdatedById { get; set; } = null!;
        public BugUser LastUpdatedBy { get; set; } = null!;

        [MaxLength(BugValidation.MaxLength)]
        public string Description { get; set; } = null!;

        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; } = null!;
        public BugUser Creator { get; set; } = null!;

        [ForeignKey(nameof(Assignee))]
        public string? AssigneeId { get; set; }
        public BugUser? Assignee { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

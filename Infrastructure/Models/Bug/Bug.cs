using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Bug
{
    public class Bug
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public BugStatus Status { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public BugPriority Priority { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}

using Core.Entities.BugEntity;
using Core.Entities.CommentEntity;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.UserEntity
{
    public class BugUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public ICollection<Bug> CreatedBugs { get; set; } = new List<Bug>();
        public ICollection<Bug> AssignedBugs { get; set; } = new List<Bug>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

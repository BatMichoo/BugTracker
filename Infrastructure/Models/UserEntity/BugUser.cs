using Infrastructure.Models.BugEntity;
using Infrastructure.Models.CommentEntity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models.UserEntity
{
    public class BugUser : IdentityUser
    {
        public string? Name { get; set; } 
        public ICollection<Bug> CreatedBugs { get; set; } = new List<Bug>();
        public ICollection<Bug> AssignedBugs { get; set; } = new List<Bug>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

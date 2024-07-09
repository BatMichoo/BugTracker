using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models.User
{
    public class BugUser : IdentityUser
    {
        public string? Name { get; set; } 
        public ICollection<Bug.Bug> CreatedBugs { get; set; } = new List<Bug.Bug>();
        public ICollection<Bug.Bug> AssignedBugs { get; set; } = new List<Bug.Bug>();
    }
}

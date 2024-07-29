using Core.DTOs.Bugs;

namespace Core.DTOs.Users
{
    public class UserCreatedBugsModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<BugModel> CreatedBugs { get; set; } = new List<BugModel>();
    }
}

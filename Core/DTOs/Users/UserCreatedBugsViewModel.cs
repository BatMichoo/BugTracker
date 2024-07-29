using Core.DTOs.Bugs;

namespace Core.DTOs.Users
{
    public class UserCreatedBugsViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<BugViewModel> CreatedBugs { get; set; } = new List<BugViewModel>();
    }
}

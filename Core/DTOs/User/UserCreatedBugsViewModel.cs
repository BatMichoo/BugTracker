using Core.DTOs.Bug;

namespace Core.DTOs.User
{
    public class UserCreatedBugsViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<BugViewModel> CreatedBugs { get; set; } = new List<BugViewModel>();
    }
}

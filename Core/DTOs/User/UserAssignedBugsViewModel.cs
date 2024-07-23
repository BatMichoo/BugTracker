using Core.DTOs.Bug;

namespace Core.DTOs.User
{
    public class UserAssignedBugsViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<BugViewModel> AssignedBugs { get; set; } = new List<BugViewModel>();
    }
}

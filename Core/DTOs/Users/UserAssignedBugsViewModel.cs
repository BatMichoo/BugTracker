using Core.DTOs.Bugs;

namespace Core.DTOs.Users
{
    public class UserAssignedBugsViewModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<BugViewModel> AssignedBugs { get; set; } = new List<BugViewModel>();
    }
}

using Core.DTOs.Bug;
using Core.Models.Bug.BugEnums;

namespace Core.BugService
{
    public interface IBugService
    {
        Task<BugViewModel> RetrieveBug(int bugId);
        Task<List<BugViewModel>> RetrieveAllActiveBugs();
        Task<List<BugViewModel>> RetrieveBugsByStatus(BugStatus status);
        Task<BugViewModel> AddBug(AddBugModel bug);
        Task<bool> DeleteBug(int bugId);
        Task<BugViewModel> UpdateOrCreateBug(EditBugViewModel bug);
        Task<BugViewModel> CloseBugAfterFixing(int bugId);
        Task<bool> ReassignBug(int bugId, string userId);
    }
}

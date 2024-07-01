using Core.DTOs.Bug;
using Infrastructure.Models.Bug;

namespace Core.BugService
{
    public interface IBugService
    {
        Task<BugViewModel> RetrieveBug(int bugId);
        Task<List<BugViewModel>> RetrieveAllBugs();
        Task<List<BugViewModel>> RetrieveBugsByStatus(BugStatus status);
        Task<BugViewModel> AddBug(AddBugViewModel bug);
        Task<bool> DeleteBug(int bugId);
        Task<BugViewModel> UpdateOrCreateBug(EditBugViewModel bug);
        Task<BugViewModel> CloseBugAfterFixing(int bugId);
        Task<bool> ReassignBug(int bugId, int userId);
    }
}

using Core.DTOs.Bug;

namespace Core.DbService
{
    public interface ITrackerDbService
    {
        Task<BugViewModel> AddBug(BugViewModel bugViewModel);
        Task<BugViewModel?> GetBug(int bugId);
        Task<BugViewModel> EditBug(BugViewModel bugViewModel);
        Task<bool> DeleteBug(int bugId);
    }
}

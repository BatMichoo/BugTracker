using Core.DTOs.Bug;
using Infrastructure.Models.Bug;

namespace Core.DbService
{
    public interface ITrackerDbService
    {
        Task<BugModel> AddBug(BugModel bug);
        Task<BugModel?> GetBug(int bugId);
        Task<BugModel> UpdateBug(BugModel bug);
        Task<bool> DeleteBug(int bugId);
        Task<List<BugModel>> GetBugsWithStatus(BugStatus? status);
    }
}

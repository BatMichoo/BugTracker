using Core.DTOs;
using Core.DTOs.Bugs;

namespace Core.BugService
{
    public interface IBugService
    {
        Task<PagedList<BugModel>> GetBugs(int page, int pageSize, string? searchTerm, string? sortOptions, string? filter);
        Task<BugModel?> GetBugById(int id);
        Task<bool> DoesExist(int id);
        Task<PagedList<BugModel>> GetAll();
        Task<PagedList<BugModel>> GetAssignedToUserId(string userId);
        Task<PagedList<BugModel>> GetCreatedByUserId(string userId);
        Task<PagedList<BugModel>> GetUnassigned();
        Task<PagedList<BugModel>> GetAll(DateTime startDate, DateTime endDate);
        Task<BugModel> CreateBug(AddBugModel model);
        Task<BugModel> UpdateBug(EditBugViewModel model);
        Task DeleteBug(int id);
    }
}

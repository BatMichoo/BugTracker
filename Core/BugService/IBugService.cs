using Core.DTOs.Bug;

namespace Core.BugService
{
    public interface IBugService
    {
        Task<PagedList<BugModel>> FetchBugs(int page, int pageSize, string searchTerm, string sortOptions, string filter);
        Task<BugModel?> FetchBugById(int id);
        Task<BugModel> CreateBug(AddBugModel model);
        Task<BugModel> UpdateBug(BugViewModel model);
        Task DeleteBug(int id);
    }
}

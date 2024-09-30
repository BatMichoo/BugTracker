using Core.BaseService;
using Core.DTOs;
using Core.DTOs.Bugs;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public interface IBugService : IAdvancedService<Bug, BugSortBy, BugFilterType, BugModel, AddBugModel, EditBugModel>
    {
        Task<bool> DoesExist(int id);
        Task<PagedList<BugModel>> GetAssignedToUserId(string userId);
        Task<PagedList<BugModel>> GetCreatedByUserId(string userId);
        Task<PagedList<BugModel>> GetUnassigned();
        Task<PagedList<BugModel>> GetAllBetweenTwoDates(DateTime startDate, DateTime endDate);
    }
}

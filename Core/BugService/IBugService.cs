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
        Task<List<BugModel>> GetAssignedToUserId(string userId);
        Task<List<BugModel>> GetCreatedByUserId(string userId);
        Task<List<BugModel>> GetUnassigned();
        Task<List<BugModel>> GetAllBetweenTwoDates(DateTime startDate, DateTime endDate);
    }
}

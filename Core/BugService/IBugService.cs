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
    }
}

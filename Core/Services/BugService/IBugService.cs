using Core.DTOs.Bugs;
using Core.Entities.BugEntity;
using Core.Services.EntityService;

namespace Core.Services.BugService
{
    public interface IBugService : IQueryEntityService<Bug, BugModel, AddBugModel, EditBugModel>
    {
        public Task<string?> GetAssigneeId(int bugId);
    }
}

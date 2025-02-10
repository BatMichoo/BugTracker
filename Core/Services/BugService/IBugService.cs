using Core.DTOs.Bugs;
using Core.Entities.BugEntity;
using Core.Services.EntityService;

namespace Core.Services.BugService
{
    public interface IBugService : IEntityService<Bug, BugModel, AddBugModel, EditBugModel>
    {
    }
}

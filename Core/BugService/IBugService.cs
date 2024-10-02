using Core.DTOs.Bugs;
using Core.EntityService;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public interface IBugService : IEntityService<Bug, BugModel, AddBugModel, EditBugModel>
    {
    }
}

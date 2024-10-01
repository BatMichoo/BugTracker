using Core.BaseService;
using Core.DTOs.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public interface IBugService : IEntityService<Bug, BugModel, AddBugModel, EditBugModel>
    {
    }
}

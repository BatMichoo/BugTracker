using Core.DTOs.Bugs;
using Core.Services.EntityService;
using Infrastructure.Models.BugEntity;

namespace Core.Services.BugService
{
    public interface IBugService : IEntityService<Bug, BugModel, AddBugModel, EditBugModel>
    {
    }
}

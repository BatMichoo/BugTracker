using AutoMapper;
using Core.DTOs.Bugs;
using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.Bugs;
using Core.Repositories;
using Core.Services.EntityService;

namespace Core.Services.BugService
{
    public class BugService : QueryEntityService<Bug, BugModel, AddBugModel, EditBugModel, BugSortBy, BugFilterType>, IBugService
    {
        public BugService(IBugRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}

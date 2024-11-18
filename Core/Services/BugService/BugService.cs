using AutoMapper;
using Core.DTOs.Bugs;
using Core.EntitiesQueryUtilities.Bugs;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;
using Core.Repository.BugRepo;
using Core.Services.EntityService;
using Infrastructure.Models.BugEntity;

namespace Core.Services.BugService
{
    public class BugService : EntityService<Bug, BugModel, AddBugModel, EditBugModel, BugSortBy, BugFilterType>, IBugService
    {
        public BugService(IBugRepository repository, IBugQueryParametersFactory queryParametersFactory, IMapper mapper)
            : base(repository, mapper, queryParametersFactory)
        {
        }
    }
}

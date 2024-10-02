using AutoMapper;
using Core.DTOs.Bugs;
using Core.EntityService;
using Core.QueryParameters;
using Core.Repository.BugRepo;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public class BugService : EntityService<Bug, BugModel, AddBugModel, EditBugModel, BugSortBy, BugFilterType>, IBugService
    {
        public BugService(IBugRepository repository, IBugQueryParametersFactory queryParametersFactory, IMapper mapper) 
            : base(repository, mapper, queryParametersFactory)
        {
        }
    }
}

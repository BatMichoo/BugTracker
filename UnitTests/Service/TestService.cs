using AutoMapper;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;
using Core.Repositories;
using Core.Services.BugService;

namespace UnitTests.Service
{
    public class TestService : BugService
    {
        public TestService(IBugRepository repository, IBugQueryParametersFactory queryParametersFactory, IMapper mapper)
            : base(repository, queryParametersFactory, mapper)
        {
        }
    }
}

using AutoMapper;
using Core.QueryParameters.Bug;
using Core.Repository.BugRepo;
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

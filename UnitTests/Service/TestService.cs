using AutoMapper;
using Core.BugService;
using Core.QueryParameters;
using Core.Repository.BugRepo;

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

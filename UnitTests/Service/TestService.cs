using AutoMapper;
using Core.Repositories;
using Core.Services.BugService;

namespace UnitTests.Service
{
    public class TestService : BugService
    {
        public TestService(IBugRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}

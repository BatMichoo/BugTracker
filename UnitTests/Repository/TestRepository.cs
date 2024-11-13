using Core.EntitiesQueryUtilities.QueryBuilders.Bugs;
using Core.Repository.BugRepo;
using Infrastructure;

namespace UnitTests.Repository
{
    public class TestRepository : BugRepository
    {
        public TestRepository(TrackerDbContext dbContext, IBugQueryableBuilder bugQueryableBuilder)
            : base(dbContext, bugQueryableBuilder)
        {
        }

    }
}

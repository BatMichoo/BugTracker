using Core.EntitiesQueryUtilities.QueryBuilders.Bugs;
using Infrastructure;
using Infrastructure.Repositories;

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

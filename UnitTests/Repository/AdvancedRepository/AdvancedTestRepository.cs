using Core.Repository.BugRepo;
using Infrastructure;

namespace UnitTests.Repository.AdvancedRepository
{
    public class AdvancedTestRepository : BugRepository
    {
        public AdvancedTestRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }
    }
}

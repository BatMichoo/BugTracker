using Core.Repository;
using Infrastructure;
using Infrastructure.Models.BugEntity;

namespace UnitTests.Repository
{
    public class SimpleTestRepository : Repository<Bug> 
    {
        public SimpleTestRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

    }
}

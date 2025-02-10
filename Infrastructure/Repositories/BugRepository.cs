using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.QueryBuilders.Bugs;
using Core.Repository.BugRepo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BugRepository : Repository<Bug>, IBugRepository
    {
        public BugRepository(TrackerDbContext dbContext, IBugQueryableBuilder queryableBuilder)
            : base(dbContext, queryableBuilder)
        {
        }

        protected override IQueryable<Bug> AddInclusions(IQueryable<Bug> query)
        {
            query = query
                .Include(b => b.Creator)
                .Include(b => b.Assignee)
                .Include(b => b.LastUpdatedBy)
                .Include(b => b.Comments)
                .AsSplitQuery();

            return query;
        }
    }
}

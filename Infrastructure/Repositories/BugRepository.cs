using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.QueryBuilders;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BugRepository : QueryRepository<Bug>, IBugRepository
    {
        public BugRepository(TrackerDbContext dbContext, IBugQueryableBuilder queryableBuilder)
            : base(dbContext, queryableBuilder)
        {
        }

        protected override IQueryable<Bug> AddInclusions(IQueryable<Bug> query, bool isFullyIncluded = false)
        {
            query = query
                .Include(b => b.Creator)
                .Include(b => b.Assignee)
                .Include(b => b.LastUpdatedBy);

            if (isFullyIncluded) 
            {
                query = query
                    .Include(b => b.Comments);
            }

            return query.AsSplitQuery();
        }
    }
}

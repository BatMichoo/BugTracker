using Core.QueryBuilders;
using Infrastructure;
using Infrastructure.Models.BugEntity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository.BugRepo
{
    public class BugRepository : Repository<Bug>, IBugRepository
    {
        public BugRepository(TrackerDbContext dbContext, IBugQueryableBuilder queryableBuilder)
            : base(dbContext, queryableBuilder)
        {
        }        

        internal override IQueryable<Bug> AddInclusions(IQueryable<Bug> query)
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

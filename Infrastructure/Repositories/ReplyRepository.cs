using Core.Entities.ReplyEntity;
using Core.EntitiesQueryUtilities.QueryBuilders;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(TrackerDbContext dbContext, IReplyQueryableBuilder queryableBuilder)
            : base(dbContext, queryableBuilder)
        {
        }

        protected override IQueryable<Reply> AddInclusions(IQueryable<Reply> query)
            => query.Include(r => r.Author);
    }
}

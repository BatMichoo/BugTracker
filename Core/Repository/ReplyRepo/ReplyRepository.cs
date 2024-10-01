using Core.QueryBuilders;
using Infrastructure;
using Infrastructure.Models.ReplyEntity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository.ReplyRepo
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(TrackerDbContext dbContext, IReplyQueryableBuilder queryableBuilder)
            : base(dbContext, queryableBuilder)
        {
        }

        internal override IQueryable<Reply> AddInclusions(IQueryable<Reply> query)
            => query.Include(r => r.Author);
    }
}

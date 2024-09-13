using Infrastructure;
using Infrastructure.Models.ReplyEntity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository.ReplyRepo
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        internal override IQueryable<Reply> AddInclusions(IQueryable<Reply> query)
            => query.Include(r => r.Author);
    }
}

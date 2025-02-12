using Core.Entities.ReplyEntity;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        public ReplyRepository(TrackerDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<Reply>> GetByCommentId(int commentId)
        {
            var entities = await AddInclusions(AsQueryable())
                .Where(r => r.CommentId == commentId)
                .ToListAsync();

            return entities;
        }

        protected override IQueryable<Reply> AddInclusions(IQueryable<Reply> query)
            => query.Include(r => r.Author);
    }
}

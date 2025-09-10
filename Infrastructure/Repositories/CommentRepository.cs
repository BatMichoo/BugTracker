using Core.Entities.CommentEntity;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(TrackerDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<Comment>> GetByBugId(int bugId, bool isFullyIncluded)
        {
            var entities = await AddInclusions(AsQueryable(), isFullyIncluded)
                .Where(c => c.BugId == bugId)
                .ToListAsync();

            return entities;
        }

        protected override IQueryable<Comment> AddInclusions(IQueryable<Comment> query, bool isFullyIncluded)
            => query.Include(c => c.Author);
    }
}

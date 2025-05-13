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

        public async Task<List<Comment>> GetByBugId(int bugId)
        {
            var entities = await AddInclusions(AsQueryable())
                .Where(c => c.BugId == bugId)
                .ToListAsync();

            return entities;
        }

        protected override IQueryable<Comment> AddInclusions(IQueryable<Comment> query, bool isFullyIncluded = false)
            => query.Include(c => c.Author);
    }
}

using Infrastructure;
using Infrastructure.Models.CommentEntity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository.CommentRepo
{
    public class CommentRepository : AdvancedRepository<Comment>, ICommentRepository
    {
        public CommentRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        internal override IQueryable<Comment> AddInclusions(IQueryable<Comment> query)
            => query.Include(c => c.Author);

        internal override IQueryable<Comment> ApplySearch(IQueryable<Comment> query, string searchTerm)
            => query.Where(c => c.Content.Contains(searchTerm));
    }
}

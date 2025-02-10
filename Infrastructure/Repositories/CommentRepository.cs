using Core.Entities.CommentEntity;
using Core.EntitiesQueryUtilities.QueryBuilders.Comments;
using Core.Repository.CommentRepo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(TrackerDbContext dbContext, ICommentQueryableBuilder queryableBuilder)
            : base(dbContext, queryableBuilder)
        {
        }

        protected override IQueryable<Comment> AddInclusions(IQueryable<Comment> query)
            => query.Include(c => c.Author);
    }
}

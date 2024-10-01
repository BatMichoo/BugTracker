using Core.QueryBuilders;
using Core.QueryParameters;
using Infrastructure;
using Infrastructure.Models.CommentEntity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository.CommentRepo
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(TrackerDbContext dbContext, ICommentQueryableBuilder queryableBuilder) 
            : base(dbContext, queryableBuilder)
        {
        }

        internal override IQueryable<Comment> AddInclusions(IQueryable<Comment> query)
            => query.Include(c => c.Author);        
    }
}

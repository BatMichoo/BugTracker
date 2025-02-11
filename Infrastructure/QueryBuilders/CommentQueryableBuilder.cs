using Core.Entities.CommentEntity;
using Core.EntitiesQueryUtilities.QueryBuilders;

namespace Infrastructure.QueryBuilders
{
    public class CommentQueryableBuilder : QueryableBuilder<Comment>, ICommentQueryableBuilder
    {
        protected override IQueryable<Comment> ApplySearch(IQueryable<Comment> query, string searchTerm)
            => query.Where(b => b.Content.Contains(searchTerm));
    }
}

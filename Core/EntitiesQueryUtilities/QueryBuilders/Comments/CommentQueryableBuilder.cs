using Core.EntitiesQueryUtilities.QueryBuilders;
using Infrastructure.Models.CommentEntity;

namespace Core.EntitiesQueryUtilities.QueryBuilders.Comments
{
    public class CommentQueryableBuilder : QueryableBuilder<Comment>, ICommentQueryableBuilder
    {
        protected override IQueryable<Comment> ApplySearch(IQueryable<Comment> query, string searchTerm)
            => query.Where(b => b.Content.Contains(searchTerm));
    }
}

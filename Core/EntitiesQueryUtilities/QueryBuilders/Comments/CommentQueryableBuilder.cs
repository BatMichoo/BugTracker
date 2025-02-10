using Core.Entities.CommentEntity;

namespace Core.EntitiesQueryUtilities.QueryBuilders.Comments
{
    public class CommentQueryableBuilder : QueryableBuilder<Comment>, ICommentQueryableBuilder
    {
        protected override IQueryable<Comment> ApplySearch(IQueryable<Comment> query, string searchTerm)
            => query.Where(b => b.Content.Contains(searchTerm));
    }
}

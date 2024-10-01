using Infrastructure.Models.CommentEntity;

namespace Core.QueryBuilders
{
    public class CommentQueryableBuilder : QueryableBuilder<Comment>, ICommentQueryableBuilder
    {
        protected override IQueryable<Comment> ApplySearch(IQueryable<Comment> query, string searchTerm)
            => query.Where(b => b.Content.Contains(searchTerm));
    }
}

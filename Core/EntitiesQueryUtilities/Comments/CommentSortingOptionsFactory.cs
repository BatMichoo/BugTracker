using Infrastructure.Models.CommentEntity;

namespace Core.Utilities.Comments
{
    public class CommentSortingOptionsFactory : ICommentSortingOptionsFactory
    {
        public ISortingOptions<Comment> CreateSortingOptions(string? sortOptions)
        {
            throw new NotImplementedException();
        }

        public ISortingOptions<Comment> CreateSortingOptions(SortOrder order, CommentOrderBy orderBy)
            => new CommentSortingOptions(order, orderBy);
    }
}

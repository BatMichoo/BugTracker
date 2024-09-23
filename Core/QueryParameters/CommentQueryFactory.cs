using Core.Utilities;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.QueryParameters
{
    public sealed class CommentQueryFactory : QueryFactory<Comment, CommentOrderBy, CommentFilterType>, ICommentQueryFactory
    {
        public CommentQueryFactory(ICommentSortingOptionsFactory sortingOptionsFactory, ICommentFilterFactory filterFactory) : base(sortingOptionsFactory, filterFactory)
        {
        }

        public QueryParameters<Comment> GetByBugId(int bugId)
        {
            var filter = _filterFactory.CreateFilter(CommentFilterType.BugId, bugId.ToString());
            var sortingOptions = _sortingOptionsFactory.CreateSortingOptions(SortOrder.Ascending, CommentOrderBy.Id);

            var filterList = new List<IFilter<Comment>>() { filter };

            return new QueryParameters<Comment>(filterList, sortOptions: sortingOptions);
        }
    }
}

using Core.Entities.CommentEntity;
using Core.EntitiesQueryUtilities.Comments;

namespace Core.EntitiesQueryUtilities.QueryParameters.Comments
{
    public class CommentQueryParametersFactory : QueryParametersFactory<Comment, CommentOrderBy, CommentFilterType>, ICommentQueryParametersFactory
    {
        public CommentQueryParametersFactory(ICommentSortingOptionsFactory sortingOptionsFactory, ICommentFilterFactory filterFactory) : base(sortingOptionsFactory, filterFactory)
        {
        }

        public QueryParameters<Comment> GetByBugId(int bugId)
        {
            var filter = _filterFactory.CreateFilter(CommentFilterType.BugId, bugId.ToString());
            var sortingOptions = new List<ISortingOptions<Comment>>() 
            {
                _sortingOptionsFactory.CreateSortingOptions(SortOrder.Ascending, CommentOrderBy.Id)
            };

            var filterList = new List<IFilter<Comment>>() { filter };

            return new QueryParameters<Comment>(filterList, sortOptions: sortingOptions);
        }
    }
}

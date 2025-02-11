using Core.Entities.CommentEntity;

namespace Core.EntitiesQueryUtilities.Comments
{
    public class CommentSortingOptionsFactory : ICommentSortingOptionsFactory
    {
        public ISortingOptions<Comment> CreateSortingOptions(string? sortOptions)
        {
            if (sortOptions != null)
            {
                string[] sortingInfo = sortOptions.Split(FilterQuerySeparators.KeyValue);

                string sortBy = sortingInfo[0];
                string order = sortingInfo[1];

                if (Enum.TryParse(sortBy, true, out CommentOrderBy sortingBy))
                {
                    if (Enum.TryParse(order, true, out SortOrder sortOrder))
                    {
                        return new CommentSortingOptions(sortOrder, sortingBy);
                    }
                }
            }

            return new CommentSortingOptions(SortOrder.Ascending, CommentOrderBy.Id);
        }

        public ISortingOptions<Comment> CreateSortingOptions(SortOrder order, CommentOrderBy orderBy)
            => new CommentSortingOptions(order, orderBy);

        IList<ISortingOptions<Comment>> ISortingOptionsFactory<Comment, CommentOrderBy>.CreateSortingOptions(string? sortOptions)
        {
            throw new NotImplementedException();
        }
    }
}

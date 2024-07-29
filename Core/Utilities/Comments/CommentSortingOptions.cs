using Infrastructure.Models.CommentEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Comments
{
    public class CommentSortingOptions : ISortingOptions<Comment>
    {
        public CommentSortingOptions(SortOrder sortOrder, CommentOrderBy orderBy)
        {
            SortOrder = sortOrder;
            OrderBy = orderBy;
        }

        public SortOrder SortOrder { get; }
        public CommentOrderBy OrderBy { get; }

        public Expression<Func<Comment, object>> Sort()
        {
            switch (OrderBy)
            {
                default:
                    return c => c.BugId;
            }
        }
    }
}

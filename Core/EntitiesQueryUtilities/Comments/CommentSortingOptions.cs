using Core.Entities.CommentEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Comments
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

        public string SortingOn => OrderBy.ToString();

        public Expression<Func<Comment, object>> Sort()
        {
            switch (OrderBy)
            {
                case CommentOrderBy.BugId:
                    return c => c.BugId;
                case CommentOrderBy.Date:
                    return c => c.PostedOn;
                case CommentOrderBy.Likes:
                    return c => c.Likes;
                default:
                    return c => c.Id;
            }
        }
    }
}

using Infrastructure.Models.BugEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public class BugSortingOptions : ISortingOptions<Bug>
    {
        public BugSortingOptions(SortOrder sortOrder, BugOrderBy sortBy)
        {
            SortOrder = sortOrder;
            SortBy = sortBy;
        }

        public SortOrder SortOrder { get; private set; }
        public BugOrderBy SortBy { get; private set; }

        public Expression<Func<Bug, object>> Sort()
        {
            switch (SortBy)
            {
                case BugOrderBy.CreatedOn:
                    return b => b.CreatedOn;
                case BugOrderBy.LastModifiedOn:
                    return b => b.LastUpdatedOn;
                case BugOrderBy.Comments:
                    return b => b.Comments.Count;
                default:
                    return b => b.Id;
            }
        }
    }
}

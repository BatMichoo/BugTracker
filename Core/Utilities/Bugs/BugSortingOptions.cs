using Infrastructure.Models.Bug;
using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public class BugSortingOptions : ISortingOptions<Bug>
    {
        public BugSortingOptions(SortOrder sortOrder, BugSortBy sortBy)
        {
            SortOrder = sortOrder;
            SortBy = sortBy;
        }

        public SortOrder SortOrder { get; private set; }
        public BugSortBy SortBy { get; private set; }

        public Expression<Func<Bug, object>> Sort()
        {
            switch (SortBy)
            {
                case BugSortBy.CreatedOn:
                    return b => b.CreatedOn;
                case BugSortBy.LastModifiedOn:
                    return b => b.LastUpdatedOn;
                case BugSortBy.Comments:
                    return b => b.Comments.Count;
                default:
                    return b => b.Id;
            }
        }
    }
}

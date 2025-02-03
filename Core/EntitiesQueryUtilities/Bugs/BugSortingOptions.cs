using Infrastructure.Models.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs
{
    public class BugSortingOptions : IBugSortingOptions
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
                case BugSortBy.Id:
                    return b => b.Id;
                case BugSortBy.Priority:
                    return b => b.Priority;
                default:
                    return b => b.Status;
            }
        }
    }
}

using Infrastructure.Models.Bug;
using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public static class BugSort
    {
        public static Expression<Func<Bug, object>> Sort(BugSortBy sortBy)
        {
            switch (sortBy)
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

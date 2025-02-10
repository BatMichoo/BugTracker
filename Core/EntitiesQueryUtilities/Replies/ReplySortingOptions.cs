using Core.Entities.ReplyEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Replies
{
    public class ReplySortingOptions : ISortingOptions<Reply>
    {
        public ReplySortingOptions(SortOrder sortOrder, ReplySortBy sortBy)
        {
            SortOrder = sortOrder;
            OrderBy = sortBy;
        }

        public SortOrder SortOrder { get; private set; }
        public ReplySortBy OrderBy { get; private set; }

        public Expression<Func<Reply, object>> Sort()
        {
            switch (OrderBy)
            {
                default:
                    return r => r.Id;
            }
        }
    }
}

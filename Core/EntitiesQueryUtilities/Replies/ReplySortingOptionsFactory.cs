using Core.Entities.ReplyEntity;

namespace Core.EntitiesQueryUtilities.Replies
{
    public class ReplySortingOptionsFactory : IReplySortingOptionFactory
    {
        public ISortingOptions<Reply> CreateSortingOptions(string? sortOptions = null)
        {
            throw new NotImplementedException();
        }

        public ISortingOptions<Reply> CreateSortingOptions(SortOrder order, ReplySortBy orderBy)
        {
            throw new NotImplementedException();
        }
    }
}

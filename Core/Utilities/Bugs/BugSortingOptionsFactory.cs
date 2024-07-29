using Infrastructure.Models.BugEntity;

namespace Core.Utilities.Bugs
{
    public class BugSortingOptionsFactory : IBugSortingOptionsFactory
    {
        public ISortingOptions<Bug> CreateSortingOptions(string? sortOptions)
        {
            if (sortOptions != null)
            {
                string[] sortingInfo = sortOptions.Split('_');

                return CreateSort(sortingInfo[0], sortingInfo[1]);                
            }

            return null;
        }

        internal static ISortingOptions<Bug> CreateSort(string sortBy, string order)
        {
            if (Enum.TryParse(sortBy, true, out BugOrderBy sortingBy))
            {
                if (Enum.TryParse(order, true, out SortOrder sortOrder))
                {
                    return new BugSortingOptions(sortOrder, sortingBy);
                }
            }

            return null;
        }

        public ISortingOptions<Bug> CreateSortingOptions(SortOrder order, BugOrderBy orderBy)
            => new BugSortingOptions(order, orderBy);
    }

    
}

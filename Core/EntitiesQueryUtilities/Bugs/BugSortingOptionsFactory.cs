using Infrastructure.Models.BugEntity;
using System.Globalization;

namespace Core.Utilities.Bugs
{
    public class BugSortingOptionsFactory : IBugSortingOptionsFactory
    {
        public ISortingOptions<Bug> CreateSortingOptions(string? sortOptions)
        {
            if (sortOptions != null)
            {
                string[] sortingInfo = sortOptions.Split('_');

                string sortBy = sortingInfo[0];
                string order = sortingInfo[1];

                if (Enum.TryParse(sortBy, true, out BugSortBy sortingBy))
                {
                    if (Enum.TryParse(order, true, out SortOrder sortOrder))
                    {
                        return new BugSortingOptions(sortOrder, sortingBy);
                    }
                }
            }

            return new BugSortingOptions(SortOrder.Ascending, BugSortBy.Id);
        }        

        public ISortingOptions<Bug> CreateSortingOptions(SortOrder order, BugSortBy orderBy)
            => new BugSortingOptions(order, orderBy);
    }    
}

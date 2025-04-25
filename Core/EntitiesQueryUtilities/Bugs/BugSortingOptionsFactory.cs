using Core.Entities.BugEntity;

namespace Core.EntitiesQueryUtilities.Bugs
{
    public class BugSortingOptionsFactory : IBugSortingOptionsFactory
    {
        public List<ISortingOptions<Bug>> CreateSortingOptions(string? sortOptions = null)
        {
            var sortingList = new List<ISortingOptions<Bug>>();

            if (sortOptions != null)
            {
                string[] sortingFilters = sortOptions.Split(FilterQuerySeparators.Filter);

                foreach (string filter in sortingFilters)
                {
                    string[] sortingInfo = filter.Split(FilterQuerySeparators.KeyValue);

                    string sortBy = sortingInfo[0];
                    string order = sortingInfo[1];

                    if (Enum.TryParse(sortBy, true, out BugSortBy sortingBy))
                    {
                        if (Enum.TryParse(order, true, out SortOrder sortOrder))
                        {
                            sortingList.Add(new BugSortingOptions(sortOrder, sortingBy));
                        }
                    }
                    else
                    {
                        sortingList.Add(new BugSortingOptions(SortOrder.Ascending, BugSortBy.Status));
                        break;
                    }
                }
            }
            else
            {
                sortingList.Add(new BugSortingOptions(SortOrder.Ascending, BugSortBy.Status));
                sortingList.Add(new BugSortingOptions(SortOrder.Descending, BugSortBy.Priority));
            }

            return sortingList;
        }        

        public ISortingOptions<Bug> CreateSortingOptions(SortOrder order, BugSortBy orderBy)
            => new BugSortingOptions(order, orderBy);
    }    
}

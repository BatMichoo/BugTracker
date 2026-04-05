using Core.EntitiesQueryUtilities.Bugs.Filters;
using Core.Models.Bugs.BugEnums;

namespace Core.EntitiesQueryUtilities.SavedSearches
{
    public class SavedSearchFilterFactory
    {
        public Filter CreateFilter(SearchFilterType filterBy, string? value = null)
        {
            switch (filterBy)
            {
                case SearchFilterType.AssignedTo:
                    return new BugAssignedToFilter(value);
                case SearchFilterType.CreatedBy:
                    return new BugCreatedByFilter(value!);
                case SearchFilterType.Priority:
                    if (Enum.TryParse<BugPriority>(value, out var priority))
                    {
                        return new BugPriorityFilter(priority);
                    }

                    throw new ArgumentException("Priority for filtering invalid.");
                case SearchFilterType.Status:
                    if (Enum.TryParse<BugStatus>(value, out var status))
                    {
                        return new BugStatusFilter(status);
                    }

                    throw new ArgumentException("Status for filtering invalid.");
                default:
                    throw new ArgumentException("No such filter");
            }
        }

        public List<Filter> CreateFilters(string? filterInput = null)
        {
            var filters = new List<Filter>();

            if (filterInput != null)
            {
                string[] filtersData = filterInput.Split(FilterQuerySeparators.Filter).ToArray();

                foreach (var filterData in filtersData)
                {
                    string[] filterInfo = filterData.Split(FilterQuerySeparators.KeyValue);

                    if (!Enum.TryParse(filterInfo[0], true, out SearchFilterType type))
                    {
                        continue;
                    }

                    string propertyValue = filterInfo.Length > 1 ? filterInfo[1] : string.Empty;

                    Filter filter = CreateFilter(type, propertyValue);

                    filters.Add(filter);
                }
            }

            return filters;
        }
    }
}


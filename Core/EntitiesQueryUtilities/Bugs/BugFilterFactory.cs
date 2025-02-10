using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.Bugs.Filters;
using Core.Models.Bugs.BugEnums;

namespace Core.EntitiesQueryUtilities.Bugs
{
    public class BugFilterFactory : IBugFilterFactory
    {
        public IFilter<Bug> CreateFilter(BugFilterType filterBy, string? value)
        {
            switch (filterBy)
            {
                case BugFilterType.AssignedTo:
                    return new BugAssignedToFilter(value);
                case BugFilterType.CreatedBy:
                    return new BugCreatedByFilter(value!);
                case BugFilterType.CreatedOn:
                    var info = value.Split(FilterQuerySeparators.Filter).ToArray();

                    var date = DateTime.Parse(info[0]);
                    var comparisonOperation = info[1];

                    return new BugCreatedOnFilter(date, comparisonOperation);
                case BugFilterType.Priority:
                    if (Enum.TryParse<BugPriority>(value, out var priority))
                    {
                        return new BugPriorityFilter(priority);
                    }

                    throw new ArgumentException("Priority for filtering invalid.");
                case BugFilterType.Status:
                    if (Enum.TryParse<BugStatus>(value, out var status))
                    {
                        return new BugStatusFilter(status);
                    }

                    throw new ArgumentException("Status for filtering invalid.");

                case BugFilterType.Title:
                    return new BugTitleFilter(value);
                default:
                    throw new ArgumentException("No such filter");
            }
        }

        public IList<IFilter<Bug>> CreateFilters(string? filterInput = null)
        {
            var filters = new List<IFilter<Bug>>();

            if (filterInput != null)
            {
                string[] filtersData = filterInput.Split(FilterQuerySeparators.Filter).ToArray();

                foreach (var filterData in filtersData)
                {
                    string[] filterInfo = filterData.Split(FilterQuerySeparators.KeyValue);

                    if (!Enum.TryParse(filterInfo[0], true, out BugFilterType type))
                    {
                        continue;
                    }

                    IFilter<Bug> filter = ProduceFilter(filterInfo, type);

                    filters.Add(filter);
                }
            }

            return filters;
        }

        private static IFilter<Bug> ProduceFilter(string[] filterInfo, BugFilterType type)
        {
            string propertyValue = filterInfo.Length > 1 ? filterInfo[1] : string.Empty;

            switch (type)
            {
                case BugFilterType.Id:
                    return new BugIdFilter(int.Parse(propertyValue));
                case BugFilterType.CreatedOn:
                    string operation = filterInfo.Length > 2 ?
                        filterInfo[2] : string.Empty;

                    var success = DateTime.TryParse(propertyValue, out DateTime createdOn);

                    if (!success)
                        createdOn = DateTime.Now;

                    return new BugCreatedOnFilter(createdOn, operation);
                case BugFilterType.AssignedTo:
                    return new BugAssignedToFilter(propertyValue);
                case BugFilterType.CreatedBy:
                    return new BugCreatedByFilter(propertyValue);
                case BugFilterType.Priority:
                    if (Enum.TryParse<BugPriority>(propertyValue, out var priority))
                    {
                        return new BugPriorityFilter(priority);
                    }

                    throw new ArgumentException("Priority for filtering invalid.");
                case BugFilterType.Status:
                    if (Enum.TryParse<BugStatus>(propertyValue, out var status))
                    {
                        return new BugStatusFilter(status);
                    }

                    throw new ArgumentException("Status for filtering invalid.");
                case BugFilterType.Title:
                    return new BugTitleFilter(propertyValue);
                default:
                    throw new ArgumentException("No such filter");
            }
        }
    }
}

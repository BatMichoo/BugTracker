using Core.EntitiesQueryUtilities.Bugs.Filters;
using Infrastructure.Models.BugEntity;

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
                    return new BugCreatedByFilter(value);
                case BugFilterType.CreatedOn:
                    var info = value.Split(FilterQuerySeparators.Filter).ToArray();

                    var date = DateTime.Parse(info[0]);
                    var comparisonOperation = info[1];

                    return new BugDateFilter(date, comparisonOperation);
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
            string propertyValue = filterInfo[1];

            switch (type)
            {
                case BugFilterType.CreatedOn:
                    string operation = filterInfo.Count() > 2 ?
                        filterInfo[2] : string.Empty;

                    var success = DateTime.TryParse(propertyValue, out DateTime createdOn);

                    if (!success)
                        createdOn = DateTime.UtcNow;

                    return new BugDateFilter(createdOn, operation);
                case BugFilterType.AssignedTo:
                    return new BugAssignedToFilter(propertyValue);
                case BugFilterType.CreatedBy:
                    return new BugCreatedByFilter(propertyValue);
                default:
                    throw new ArgumentException("No such filter");
            }
        }
    }
}

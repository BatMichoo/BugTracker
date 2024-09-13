using Infrastructure.Models.BugEntity;

namespace Core.Utilities.Bugs
{
    public class BugFilterFactory : IBugFilterFactory
    {
        public IFilter<Bug> CreateFilter(BugFilterType filterBy, string value)
        {
            switch (filterBy)
            {
                case BugFilterType.AssignedTo:
                    return new BugAssignedToFilter(value);
                case BugFilterType.CreatedBy:
                    return new BugCreatedByFilter(value);
                case BugFilterType.CreatedOn:
                    var info = value.Split(';').ToArray();

                    var date = DateTime.Parse(info[0]);
                    var comparisonOperation = info[1];

                    return new BugDateFilter(date, comparisonOperation);
                default:
                    throw new ArgumentException("No such filter");
            }
        }

        public Task<IList<IFilter<Bug>>> CreateFilters(string? filterInput = null)
        {
            var filters = new List<IFilter<Bug>>();

            if (filterInput != null)
            {
                string[] filtersData = filterInput.Split(';').ToArray();

                foreach (var filterData in filtersData)
                {
                    string[] filterInfo = filterData.Split("_");

                    if (!Enum.TryParse(filterInfo[0], true, out BugFilterType type))
                    {
                        continue;
                    }

                    string propertyValue = filterInfo[1];

                    IFilter<Bug> filter;

                    switch (type)
                    {
                        case BugFilterType.CreatedOn:
                            string operation = filterInfo[2];
                            var success = DateTime.TryParse(propertyValue, out DateTime createdOn);

                            if (!success)
                                createdOn = DateTime.UtcNow;

                            filter = new BugDateFilter(createdOn, operation);
                            break;
                        case BugFilterType.AssignedTo:
                            filter = new BugAssignedToFilter(propertyValue);
                            break;
                        case BugFilterType.CreatedBy:
                            filter = new BugCreatedByFilter(propertyValue);
                            break;
                        default:
                            continue;

                    }

                    filters.Add(filter);
                }
            }

            return Task.FromResult((IList<IFilter<Bug>>) filters);
        }
    }
}

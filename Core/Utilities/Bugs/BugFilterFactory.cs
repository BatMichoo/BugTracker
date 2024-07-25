using Core.Other;
using Infrastructure.Models.Bug;
using System.Reflection.Metadata.Ecma335;

namespace Core.Utilities.Bugs
{
    public class BugFilterFactory : IFilterFactory<Bug>
    {
        public Task<IList<IFilter<Bug>>> CreateFilters(string filterInput)
        {
            var filters = new List<IFilter<Bug>>();

            if (filterInput != null)
            {
                string[] filtersData = filterInput.Split(';').ToArray();

                foreach (var filterData in filtersData)
                {
                    string[] filterInfo = filterData.Split("_");

                    if (!Enum.TryParse(filterInfo[0], true, out BugFilterType type) )
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

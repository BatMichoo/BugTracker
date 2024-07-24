using Core.Other;
using Infrastructure.Models.Bug;

namespace Core.Utilities.Bugs
{
    public class BugFilterFactory : IFilterFactory<Bug>
    {
        public async Task<IList<IFilter<Bug>>> CreateFilters(string filterInput)
        {
            var filters = new List<IFilter<Bug>>();

            if (filterInput != null)
            {
                string[] filtersData = filterInput.Split(';').ToArray();

                foreach (var filterData in filtersData)
                {
                    string[] filterInfo = filterData.Split("_");

                    string propertyName = filterInfo[0];
                    string propertyValue = filterInfo[1];

                    IFilter<Bug> filter;

                    switch (propertyName)
                    {
                        case "createdon":
                            string operation = filterInfo[2];
                            filter = new BugDateFilter(DateTime.Parse(propertyValue), operation);
                            break;
                        case "assignedto":
                            filter = new BugAssignedToFilter(propertyValue);
                            break;
                        case "createdby":
                            filter = new BugCreatedByFilter(propertyValue);
                            break;
                        default:
                            continue;

                    }

                    filters.Add(filter);
                }
            }

            return filters;
        }
    }
}

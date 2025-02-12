using Core.Entities;
using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.DTOs
{
    public class QueryModel<T> where T : class
    {
        public List<FilterModel> Filters { get; set; }
        public List<SortingModel> Sortings { get; set; }
        public PagingInfo PageInfo { get; set; }
        public List<T> Items { get; set; }

        public static QueryModel<T> Parse<U>(QueryParameters<U> queryParameters, List<T> items)
            where U : BaseModel
        {
            var parsedFilters = ParseFilters(queryParameters.Filters);
            var parsedSortings = ParseSortings(queryParameters.SortOptions);

            return new QueryModel<T>
            {
                Filters = parsedFilters,
                Sortings = parsedSortings,
                PageInfo = queryParameters.PagingInfo,
                Items = items
            };
        }

        private static List<SortingModel> ParseSortings<U>(List<ISortingOptions<U>> sortings) where U : BaseModel
        {
            var sortingList = new List<SortingModel>();

            foreach (var sorting in sortings)
            {
                var newEntry = new SortingModel()
                {
                    SortingOn = sorting.SortingOn,
                    Order = sorting.SortOrder.ToString()
                };

                sortingList.Add(newEntry);
            }

            return sortingList;
        }

        private static List<FilterModel> ParseFilters<U>(List<IFilter<U>> filters) where U : BaseModel
        {
            var filterList = new List<FilterModel>();

            foreach (Filter filter in filters)
            {
                var newEntry = new FilterModel()
                {
                    Name = filter.Name,
                    Value = filter.Value
                };

                filterList.Add(newEntry);
            }

            return filterList;
        }
    }
}

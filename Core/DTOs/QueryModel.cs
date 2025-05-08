using Core.Entities;
using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.DTOs
{
    public class QueryModel<T> where T : class
    {
        public Dictionary<string, string> Filters { get; set; }
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

        private static Dictionary<string, string> ParseFilters<U>(List<IFilter<U>> filters) where U : BaseModel
        {
            var filterList = new Dictionary<string, string>();

            foreach (Filter filter in filters)
            {
                var nameCharArr = filter.Name.ToCharArray();
                nameCharArr[0] = char.ToLower(nameCharArr[0]);

                string name = string.Join(string.Empty, nameCharArr);

                filterList.Add(name, filter.Value);
            }

            return filterList;
        }
    }
}

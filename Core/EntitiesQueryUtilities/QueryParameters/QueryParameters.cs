using Core.Entities;

namespace Core.EntitiesQueryUtilities.QueryParameters
{
    public class QueryParameters<T> where T : BaseModel
    {
        public QueryParameters(List<IFilter<T>>? filters = null, PagingInfo? pagingInfo = null, List<ISortingOptions<T>>? sortOptions = null, string? searchTerm = null)
        {
            Filters = filters ?? new List<IFilter<T>>();
            PagingInfo = pagingInfo ?? PagingInfo.CreatePage(pageNumber: PagingDefaults.StartingPageNumber, elementsPerPage: PagingDefaults.ElementsPerPage);
            SearchTerm = searchTerm;
            SortOptions = sortOptions ?? new List<ISortingOptions<T>>();
        }

        public List<IFilter<T>> Filters { get; }
        public PagingInfo PagingInfo { get; }
        public string? SearchTerm { get; }
        public List<ISortingOptions<T>> SortOptions { get; }
    }
}

using Core.Utilities;
using Infrastructure.Models;

namespace Core.QueryParameters
{
    public class QueryParameters<T> where T : BaseEntity
    {
        public QueryParameters(IList<IFilter<T>> filters, PagingInfo? pagingInfo = null, ISortingOptions<T> sortOptions = null, string? searchTerm = null)
        {
            Filters = filters ?? new List<IFilter<T>>();
            PagingInfo = pagingInfo ?? PagingInfo.CreatePage(pageNumber: PagingDefaults.StartingPageNumber, elementsPerPage: PagingDefaults.ElementsPerPage);
            SearchTerm = searchTerm;
            SortOptions = sortOptions;
        }

        public IList<IFilter<T>> Filters { get; } 
        public PagingInfo PagingInfo { get; } 
        public string? SearchTerm { get; } 
        public ISortingOptions<T> SortOptions { get; }         
    }
}

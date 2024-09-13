using Core.Utilities;
using Infrastructure.Models;

namespace Core.QueryParameters
{
    public class QueryParameters<T> where T : BaseEntity
    {
        public QueryParameters(IList<IFilter<T>> filters = null, PagingInfo pagingInfo = null, string? searchTerm = null, ISortingOptions<T>? sortOptions = null)
        {
            Filters = filters ?? new List<IFilter<T>>();
            PagingInfo = pagingInfo ??  new PagingInfo();
            SearchTerm = searchTerm;
            SortOptions = sortOptions;
        }

        public IList<IFilter<T>> Filters { get; } 
        public PagingInfo PagingInfo { get; } 
        public string? SearchTerm { get; } 
        public ISortingOptions<T>? SortOptions { get; }         
    }
}

using Core.Utilities;
using Infrastructure.Models;

namespace Core.QueryParameters
{
    public abstract class QueryParametersFactory<TEntity, TSortBy, TFilterBy> : IQueryParametersFactory<TEntity, TSortBy, TFilterBy>
        where TEntity : BaseEntity
        where TSortBy : struct, Enum
        where TFilterBy : struct, Enum
    {
        protected readonly ISortingOptionsFactory<TEntity, TSortBy> _sortingOptionsFactory;
        protected readonly IFilterFactory<TEntity, TFilterBy> _filterFactory;

        protected QueryParametersFactory(ISortingOptionsFactory<TEntity, TSortBy> sortingOptionsFactory, IFilterFactory<TEntity, TFilterBy> filterFactory)
        {
            _sortingOptionsFactory = sortingOptionsFactory;
            _filterFactory = filterFactory;
        }

        public Task<QueryParameters<TEntity>> CreateGetAllQuery()
        {
            return Task.FromResult(new QueryParameters<TEntity>());
        }

        public Task<QueryParameters<TEntity>> ProcessQueryParametersInput(int pageInput, int pageSizeInput, string? searchTermInput,
            string? sortOptionsInput, string? filterInput)
        {
            var filters = _filterFactory.CreateFilters(filterInput);

            var sortingOptions = _sortingOptionsFactory.CreateSortingOptions(sortOptionsInput);

            var pageInfo = PagingInfo.CreatePage(pageInput, pageSizeInput);

            return Task.FromResult(new QueryParameters<TEntity>(filters, pageInfo, sortingOptions, searchTermInput));
        }
    }
}

using AutoMapper;
using Core.DTOs;
using Core.QueryParameters;
using Core.Repository;
using Core.Utilities;
using Infrastructure.Models;

namespace Core.BaseService
{
    public abstract class AdvancedService<TEntity, TOrderEnum, TFilterBy, TModel, TCreate, TUpdate> :
        SimpleService<TEntity, TModel, TCreate, TUpdate>,
        IAdvancedService<TEntity, TOrderEnum, TFilterBy, TModel, TCreate, TUpdate>

        where TEntity : BaseEntity
        where TOrderEnum : struct, Enum
        where TFilterBy : struct, Enum
        where TModel : class
        where TCreate : class
        where TUpdate : class
    {
        protected readonly IFilterFactory<TEntity, TFilterBy> _filterFactory;
        protected readonly ISortingOptionsFactory<TEntity, TOrderEnum> _sortingOptionsFactory;
        protected IAdvancedRepository<TEntity> _advancedRepository => (IAdvancedRepository<TEntity>) _repository;

        public AdvancedService(IAdvancedRepository<TEntity> repository, IMapper mapper, ISortingOptionsFactory<TEntity, TOrderEnum> sortingOptionsFactory, IFilterFactory<TEntity, TFilterBy> filterFactory) : base(repository, mapper)
        {
            _sortingOptionsFactory = sortingOptionsFactory;
            _filterFactory = filterFactory;
        }        

        public async Task<PagedList<TModel>> Fetch(int pageInput, int pageSizeInput, string? searchTermInput, string? sortOptionsInput, string? filterInput)
        {
            var queryParameters = await ProcessQueryParametersInput(pageInput, pageSizeInput, searchTermInput, sortOptionsInput, filterInput);

            var bugList = await _advancedRepository.RunQuery(queryParameters);

            return new PagedList<TModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<TModel>>(bugList),
            };
        }

        protected async Task<QueryParameters<TEntity>> ProcessQueryParametersInput(int pageInput, int pageSizeInput, string? searchTermInput,
            string? sortOptionsInput, string? filterInput)
        {
            var filters = await _filterFactory.CreateFilters(filterInput);
            var sortingOptions = _sortingOptionsFactory.CreateSortingOptions(sortOptionsInput);

            var pageInfo = PagingInfo.CreatePage(pageInput, pageSizeInput);

            return new QueryParameters<TEntity>(filters, pageInfo, searchTermInput, sortingOptions);
        }
    }
}

using AutoMapper;
using Core.DTOs;
using Core.QueryParameters;
using Core.Repository;
using Infrastructure.Models;

namespace Core.BaseService
{
    public abstract class AdvancedService<TEntity, TSortBy, TFilterBy, TModel, TCreate, TUpdate> :
        SimpleService<TEntity, TModel, TCreate, TUpdate>,
        IAdvancedService<TEntity, TSortBy, TFilterBy, TModel, TCreate, TUpdate>

        where TEntity : BaseEntity
        where TSortBy : struct, Enum
        where TFilterBy : struct, Enum
        where TModel : class
        where TCreate : class
        where TUpdate : class
    {
        protected IAdvancedRepository<TEntity> AdvancedRepository => (IAdvancedRepository<TEntity>) _repository;

        protected AdvancedService(IAdvancedRepository<TEntity> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }        

        public async Task<PagedList<TModel>> Fetch(QueryParameters<TEntity> queryParameters)
        {
            var bugList = await AdvancedRepository.RunQuery(queryParameters);

            return new PagedList<TModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<TModel>>(bugList),
            };
        }        
    }
}

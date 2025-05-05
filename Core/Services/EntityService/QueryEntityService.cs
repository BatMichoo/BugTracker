using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;
using Core.Repositories;

namespace Core.Services.EntityService
{
    public class QueryEntityService<TEntity, TModel, TCreate, TUpdate, TSortBy, TFilterBy> :
                                                                                             EntityService<TEntity, TModel, TCreate, TUpdate, TSortBy, TFilterBy>,
                                                                                             IQueryEntityService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseModel
        where TModel : class
        where TCreate : class
        where TUpdate : BaseModel
        where TSortBy : struct, Enum
        where TFilterBy : struct, Enum
    {
        public QueryEntityService(IQueryRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<QueryModel<TModel>> Fetch(QueryParameters<TEntity> queryParameters)
        {
            var totalElementCount = await ((IQueryRepository<TEntity>)_repository).Count(queryParameters);
            queryParameters.PagingInfo.UpdatePaging(totalElementCount);

            var entitiesList = await ((IQueryRepository<TEntity>)_repository).ExecuteQuery(queryParameters);

            var queriedEntities = QueryModel<TModel>.Parse(queryParameters, _mapper.Map<List<TModel>>(entitiesList));

            return queriedEntities;
        }
    }
}

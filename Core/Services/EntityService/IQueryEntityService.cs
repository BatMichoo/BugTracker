using Core.DTOs;
using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.Services.EntityService
{
    public interface IQueryEntityService<TEntity, TModel, TCreate, TUpdate>: IEntityService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseModel
        where TModel : class
        where TCreate : class
        where TUpdate : class
    {
        abstract Task<QueryModel<TModel>> Fetch(QueryParameters<TEntity> queryParameters);
    }
}

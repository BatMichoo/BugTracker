using Core.DTOs;
using Core.QueryParameters;
using Infrastructure.Models;

namespace Core.BaseService
{
    public interface IAdvancedService<TEntity, TOrderEnum, TFilterBy, TModel, TCreate, TUpdate> : ISimpleService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseEntity
        where TOrderEnum : struct, Enum
        where TFilterBy : struct, Enum
        where TModel : class
        where TCreate : class
        where TUpdate : class
    {
        Task<PagedList<TModel>> Fetch(QueryParameters<TEntity> queryParameters);
    }
}
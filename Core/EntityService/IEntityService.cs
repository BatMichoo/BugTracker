using Core.DTOs;
using Core.QueryParameters;
using Infrastructure.Models;

namespace Core.EntityService
{
    public interface IEntityService<TEntity, TModel, TCreate, TUpdate> 
        where TEntity : BaseEntity
        where TModel : class
        where TCreate : class 
        where TUpdate : class
    {
        Task<TModel?> GetById(int id);
        Task<List<TModel>> GetAll();
        Task<TModel> Create(TCreate createModel);
        Task<TModel> Update(TUpdate updateModel);
        Task<bool> DoesExist(int id);
        Task Delete(int id);
        abstract Task<PagedList<TModel>> Fetch(QueryParameters<TEntity> queryParameters);
    }
}

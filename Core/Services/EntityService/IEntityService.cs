using Core.DTOs;
using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.Services.EntityService
{
    public interface IEntityService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseModel
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

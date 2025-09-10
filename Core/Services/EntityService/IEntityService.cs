using Core.Entities;

namespace Core.Services.EntityService
{
    public interface IEntityService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseModel
        where TModel : class
        where TCreate : class
        where TUpdate : class
    {
        Task<TModel?> GetById(int id, bool isFullyIncluded = false);
        abstract Task<List<TModel>> GetAll();
        Task<TModel> Create(TCreate createModel);
        Task<TModel> Update(TUpdate updateModel);
        Task<bool> DoesExist(int id);
        Task Delete(int id);
    }
}

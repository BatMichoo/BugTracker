using Infrastructure.Models;

namespace Core.BaseService
{
    public interface ISimpleService<TEntity, TModel, TCreate, TUpdate> 
        where TEntity : BaseEntity
        where TModel : class
        where TCreate : class 
        where TUpdate : class
    {
        Task<TModel?> GetById(int id);
        Task<List<TModel>> GetAll();
        Task<TModel> Create(TCreate createModel);
        Task<TModel> Update(TUpdate updateModel);
        Task Delete(int id);
    }
}

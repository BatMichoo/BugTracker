using Core.Entities;

namespace Core.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T?> GetById(int id);
        abstract Task<List<T>> GetAll();
        Task<bool> DoesExist(int id);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task DeleteById(int id);
        Task Delete(T entity);
    }
}

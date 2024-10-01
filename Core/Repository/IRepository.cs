using Core.QueryParameters;
using Infrastructure.Models;

namespace Core.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(int id);
        Task<bool> DoesExist(int id);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task DeleteById(int id);
        Task Delete(T entity);

        Task<int> CountTotal();

        Task<List<T>> ExecuteQuery(QueryParameters<T> queryParameters);
        
    }
}

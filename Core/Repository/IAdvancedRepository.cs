using Core.QueryParameters;
using Infrastructure.Models;

namespace Core.Repository
{
    public interface IAdvancedRepository<T> : IRepository<T> where T : BaseEntity
    {        
        Task<IList<T>> RunQuery(QueryParameters<T> queryParameters);
    }
}

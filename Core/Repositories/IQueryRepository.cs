using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.Repositories
{
    public interface IQueryRepository<T> : IRepository<T>
        where T : BaseModel
    {
        Task<int> Count(QueryParameters<T> queryParameters);

        Task<List<T>> ExecuteQuery(QueryParameters<T> queryParameters);
    }
}

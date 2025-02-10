using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.EntitiesQueryUtilities.QueryBuilders
{
    public interface IQueryableBuilder<T> where T : BaseModel
    {
        IQueryable<T> BuildQuery(IQueryable<T> query, QueryParameters<T>? queryParameters);
        IQueryable<T> BuildCountQuery(IQueryable<T> query, QueryParameters<T>? queryParameters);
    }
}

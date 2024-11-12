using Core.EntitiesQueryUtilities.QueryParameters;
using Infrastructure.Models;

namespace Core.EntitiesQueryUtilities.QueryBuilders
{
    public interface IQueryableBuilder<T> where T : BaseEntity
    {
        IQueryable<T> BuildQuery(IQueryable<T> query, QueryParameters<T>? queryParameters);
    }
}

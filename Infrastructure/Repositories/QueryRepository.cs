using Core.Entities;
using Core.EntitiesQueryUtilities.QueryBuilders;
using Core.EntitiesQueryUtilities.QueryParameters;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class QueryRepository<T> : Repository<T>, IQueryRepository<T>
        where T : BaseModel
    {
        private readonly IQueryableBuilder<T> _queryBuilder;

        protected QueryRepository(TrackerDbContext dbContext, IQueryableBuilder<T> queryableBuilder) : base(dbContext)
        {
            _queryBuilder = queryableBuilder;
        }

        public async Task<List<T>> ExecuteQuery(QueryParameters<T> queryParameters)
        {
            var query = AddInclusions(AsQueryable());

            query = _queryBuilder.BuildQuery(query, queryParameters);

            var entityList = await query.ToListAsync();

            return entityList;
        }

        public async Task<int> Count(QueryParameters<T> queryParameters)
        {
            var query = _queryBuilder.BuildCountQuery(AsQueryable(), queryParameters);

            var count = await query.CountAsync();

            return count;
        }
    }
}

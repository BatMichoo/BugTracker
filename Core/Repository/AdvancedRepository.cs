using Core.QueryParameters;
using Core.Utilities;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public abstract class AdvancedRepository<T> : Repository<T>, IAdvancedRepository<T> where T : BaseEntity
    {
        public AdvancedRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        private static IQueryable<T> ApplyFilter(IQueryable<T> query, IList<IFilter<T>> filters)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter.Apply());
            }

            return query;
        }

        internal abstract IQueryable<T> ApplySearch(IQueryable<T> query, string searchTerm);

        private static IQueryable<T> ApplyPagination(IQueryable<T> query, PagingInfo paging)
        {
            int itemsToSkip = (paging.CurrentPage - 1) * paging.ElementsPerPage;

            if (itemsToSkip > 0)
            {
                query = query.Skip(itemsToSkip);
            }

            query = query.Take(paging.ElementsPerPage);

            return query;
        }

        private static IQueryable<T> ApplySort(IQueryable<T> query, ISortingOptions<T> sortOptions)
        {
            switch (sortOptions)
            {
                case SortOrder.Descending:
                    query = query.OrderByDescending(sortOptions.Sort());
                    break;
                default:
                    query = query.OrderBy(sortOptions.Sort());
                    break;
            }

            return query;
        }

        private IQueryable<T> BuildQuery(QueryParameters<T>? queryParameters)
        {
            var query = AsQueryable().AsNoTracking();

            if (queryParameters is not null)
            {
                if (queryParameters.Filters != null && queryParameters.Filters.Any())
                {
                    query = ApplyFilter(query, queryParameters.Filters);
                }

                if (queryParameters.SearchTerm != null)
                {
                    query = ApplySearch(query, queryParameters.SearchTerm);
                }

                if (queryParameters.SortOptions != null)
                {
                    query = ApplySort(query, queryParameters.SortOptions);
                }
            }

            query = ApplyPagination(query, queryParameters.PagingInfo);

            return query;
        }

        public async Task<IList<T>> RunQuery(QueryParameters<T>? queryParameters = null)
        {
            var query = BuildQuery(queryParameters);

            query = AddInclusions(query);
            
            var count = await query.CountAsync();

            queryParameters.PagingInfo.TotalElementCount = count;

            return await query.ToListAsync();
        }
    }
}

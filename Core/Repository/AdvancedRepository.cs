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
                query = query.Where(filter.ToExpression());
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

        private IQueryable<T> BuildQuery(IList<IFilter<T>> filters, ISortingOptions<T> sortOptions, string? searchTerm, PagingInfo? pagingInfo)
        {
            var query = AsQueryable().AsNoTracking();

            if (filters.Any())
            {
                query = ApplyFilter(query, filters);
            }

            if (searchTerm != null)
            {
                query = ApplySearch(query, searchTerm);
            }

            if (sortOptions != null)
            {
                query = ApplySort(query, sortOptions);
            }

            if (pagingInfo != null)
            {
                query = ApplyPagination(query, pagingInfo);
            }

            return query;
        }

        public async Task<IList<T>> RetrieveData(IList<IFilter<T>> filters, ISortingOptions<T> sortOptions, string? searchTerm, PagingInfo? pagingInfo)
        {
            var query = BuildQuery(filters, sortOptions, searchTerm, pagingInfo);

            query = AddInclusions(query);

            return await query.ToListAsync();
        }
    }
}

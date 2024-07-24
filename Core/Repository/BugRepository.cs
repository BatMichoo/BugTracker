using Core.Utilities.Bugs;
using Infrastructure;
using Infrastructure.Models.Bug;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class BugRepository : Repository<Bug>, IAdvancedRepository<Bug>
    {
        public BugRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        private IQueryable<Bug> ApplyFilter(IQueryable<Bug> query, IList<IFilter<Bug>> filters)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter.ToExpression());
            }

            return query;
        }

        private IQueryable<Bug> ApplySearch(IQueryable<Bug> query, string? searchTerm)
        {
            if (searchTerm != null)
            {
                query = query.Where(b => b.Description.Contains(searchTerm));  
            }
            
            return query;
        }

        private IQueryable<Bug> ApplyPagination(IQueryable<Bug> query, PagingInfo paging)
        {
            int itemsToSkip = (paging.CurrentPage - 1) * paging.ElementsPerPage;

            if (itemsToSkip > 0)
            {
                query = query.Skip(itemsToSkip);
            }

            query = query.Take(paging.ElementsPerPage);

            return query;
        }

        private IQueryable<Bug> ApplySort(IQueryable<Bug> query, BugSortingOptions sortOptions)
        {
            switch (sortOptions.SortOrder)
            {
                case BugSortOrder.Descending:
                    query = query.OrderByDescending(BugSort.Sort(sortOptions.SortBy));
                    break;
                default:
                    query = query.OrderBy(BugSort.Sort(sortOptions.SortBy));
                    break;
            }

            return query;
        }

        private IQueryable<Bug> BuildQuery(IList<IFilter<Bug>> filters, BugSortingOptions sortOptions, string? searchTerm, PagingInfo? pagingInfo)
        {
            var query = AsQueryable();

            if (filters.Any())
            {
                query = ApplyFilter(query, filters);
            }

            if (searchTerm != null)
            {
                query = ApplySearch(query, searchTerm);
            }

            query = ApplySort(query, sortOptions);

            if (pagingInfo != null)
            {
                query = ApplyPagination(query, pagingInfo);
            }

            return query;
        }

        public override async Task<Bug?> GetById(int id)
        {
            var bug = await AsQueryable()
                .Include(b => b.Creator)
                .Include(b => b.LastUpdatedBy)
                .Include(b => b.Assignee)
                .Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.Id == id);

            return bug;
        }

        public async Task<IList<Bug>> RetrieveData(IList<IFilter<Bug>> filters, BugSortingOptions sortOptions, string? searchTerm, PagingInfo? pagingInfo)
        {
            var data = await BuildQuery(filters, sortOptions, searchTerm, pagingInfo)
                .Include(b => b.Creator)
                .Include(b => b.Assignee)
                .Include(b => b.LastUpdatedBy)
                .Include(b => b.Comments)
                .ToListAsync();

            return data;
        }
    }
}

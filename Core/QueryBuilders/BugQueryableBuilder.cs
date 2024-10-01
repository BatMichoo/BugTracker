using Infrastructure.Models.BugEntity;

namespace Core.QueryBuilders
{
    public class BugQueryableBuilder : QueryableBuilder<Bug>, IBugQueryableBuilder
    {
        protected override IQueryable<Bug> ApplySearch(IQueryable<Bug> query, string searchTerm)
            => query.Where(b => b.Description.Contains(searchTerm));
    }
}

using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.QueryBuilders;

namespace Infrastructure.QueryBuilders
{
    public class BugQueryableBuilder : QueryableBuilder<Bug>, IBugQueryableBuilder
    {
        protected override IQueryable<Bug> ApplySearch(IQueryable<Bug> query, string searchTerm)
            => query.Where(b => b.Description.Contains(searchTerm));
    }
}

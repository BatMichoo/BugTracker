using Core.Entities.SearchEntity;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SearchesRepository : Repository<Search>, ISearchesRepository
    {
        public SearchesRepository(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Search>> GetForUser(string userId)
        {
            var searches = await _dbSet.AsNoTracking()
                .Where(s => s.CreatedById == userId)
                .ToListAsync();

            return searches;
        }
    }
}

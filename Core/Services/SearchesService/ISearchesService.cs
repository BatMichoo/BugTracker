using Core.Entities.SearchEntity;
using Core.Services.EntityService;

namespace Core.Services.SearchesService
{
    public interface ISearchesService : IEntityService<Search, Search, Search, Search>
    {
        public Task CreateDefaultSavedSearches(string userId);
        public Task<List<Search>> GetForUser(string userId);
    }
}

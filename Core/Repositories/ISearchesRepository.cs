using Core.Entities.SearchEntity;

namespace Core.Repositories
{
    public interface ISearchesRepository : IRepository<Search>
    {
        public Task<List<Search>> GetForUser(string userId);
    }
}

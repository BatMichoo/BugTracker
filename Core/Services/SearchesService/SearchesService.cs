using Core.Entities.SearchEntity;
using Core.Repositories;

namespace Core.Services.SearchesService
{
    public class SearchesService : ISearchesService
    {
        private readonly ISearchesRepository _repository;

        public SearchesService(ISearchesRepository searchesRepository)
        {
            _repository = searchesRepository;
        }

        public async Task<Search> Create(Search createModel)
        {
            return await _repository.Create(createModel);
        }

        public async Task<List<Search>> GetForUser(string userId)
        {
            return await _repository.GetForUser(userId);
        }

        public async Task CreateDefaultSavedSearches(string userId)
        {
            var defaultSearches = new List<Search>
            {
                new Search { Name = "Assigned to me", CreatedById = userId, QueryString = $"assignedTo_{userId}"},
                new Search { Name = "Created by me", CreatedById = userId, QueryString = $"createdBy_{userId}"},
            };

            foreach (var search in defaultSearches)
            {
                await Create(search);
            }
        }

        public async Task Delete(int id)
        {
            var toDelete = await _repository.GetById(id);

            if (toDelete is not null)
            {
                await _repository.Delete(toDelete);
            }
        }

        public async Task<bool> DoesExist(int id)
        {
            return await _repository.DoesExist(id);
        }

        public async Task<List<Search>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Search?> GetById(int id, bool isFullyIncluded)
        {
            return await _repository.GetById(id);
        }

        public async Task<Search> Update(Search updateModel)
        {
            return await _repository.Update(updateModel);
        }
    }
}

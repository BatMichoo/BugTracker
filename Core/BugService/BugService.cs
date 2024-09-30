using AutoMapper;
using Core.BaseService;
using Core.DTOs.Bugs;
using Core.Repository.BugRepo;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public class BugService : AdvancedService<Bug, BugSortBy, BugFilterType, BugModel, AddBugModel, EditBugModel>, IBugService
    {
        public BugService(IBugRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public async Task<bool> DoesExist(int id)
            => await AdvancedRepository.DoesExist(id);
                
    }
}

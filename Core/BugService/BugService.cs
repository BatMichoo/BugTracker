using AutoMapper;
using Core.DTOs.Bug;
using Core.Repository;
using Core.Utilities.Bugs;
using Infrastructure.Models.Bug;

namespace Core.BugService
{
    public class BugService : IBugService
    {
        private readonly IAdvancedRepository<Bug> bugRepository;
        private readonly IMapper mapper;
        private readonly IFilterFactory<Bug> filterFactory;

        public BugService(IAdvancedRepository<Bug> bugRepository, IMapper mapper, IFilterFactory<Bug> filterFactory)
        {
            this.bugRepository = bugRepository;
            this.mapper = mapper;
            this.filterFactory = filterFactory;
        }

        public async Task<BugModel> CreateBug(AddBugModel model)
        {
            var bugDbModel = mapper.Map<Bug>(model);

            var newBug = await bugRepository.Create(bugDbModel);

            return mapper.Map<BugModel>(newBug);
        }

        public async Task DeleteBug(int id)
        {
            await bugRepository.DeleteById(id);
        }

        public async Task<BugModel?> FetchBugById(int id)
        {
            var bug = await bugRepository.GetById(id);

            if (bug != null)
            {
                return mapper.Map<BugModel>(bug);
            }

            return null;
        }

        public async Task<PagedList<BugModel>> FetchBugs(int page, int pageSize, string searchTerm, string sortOptions, string filter)
        {
            var filters = await filterFactory.CreateFilters(filter);

            BugSortingOptions sortingOptions = new BugSortingOptions();

            if (sortOptions != null)
            {
                string[] sortingInfo = sortOptions.Split('_');

                if (Enum.TryParse(sortingInfo[0], true, out BugSortBy sortBy))
                {
                    sortingOptions.SortBy = sortBy;
                }
                if (Enum.TryParse(sortingInfo[1], true, out BugSortOrder sortOrder))
                { 
                    sortingOptions.SortOrder = sortOrder;
                }
            }

            var pageInfo = new PagingInfo() { CurrentPage = page, ElementsPerPage = pageSize };

            var bugList = await bugRepository.RetrieveData(filters, sortingOptions, searchTerm, pageInfo);

            return new PagedList<BugModel>
            {
                CurrentPage = page,
                PageSize = pageSize,
                Items = mapper.Map<List<BugModel>>(bugList)
            };
        }

        public async Task<BugModel> UpdateBug(BugViewModel model)
        {
            var bug = await bugRepository.GetById(model.Id);

            if (bug != null)
            {
                var updateBug = await bugRepository.Update(mapper.Map<Bug>(model));

                return mapper.Map<BugModel>(updateBug);
            }

            return null;
        }
    }
}

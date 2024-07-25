using AutoMapper;
using Core.DTOs;
using Core.DTOs.Bug;
using Core.Repository;
using Core.Utilities;
using Infrastructure.Models.Bug;

namespace Core.BugService
{
    public class BugService : IBugService
    {
        private readonly IAdvancedRepository<Bug> bugRepository;
        private readonly IFilterFactory<Bug> filterFactory;
        private readonly ISortingOptionsFactory<Bug> sortingFactory;
        private readonly IMapper mapper;

        public BugService(IAdvancedRepository<Bug> bugRepository, IFilterFactory<Bug> filterFactory,
            ISortingOptionsFactory<Bug> sortingFactory, IMapper mapper)
        {
            this.bugRepository = bugRepository;
            this.filterFactory = filterFactory;
            this.sortingFactory = sortingFactory;
            this.mapper = mapper;
        }

        public async Task<BugModel> CreateBug(AddBugModel model)
        {
            var bugDbModel = mapper.Map<Bug>(model);

            bugDbModel.LastUpdatedOn = bugDbModel.CreatedOn;
            bugDbModel.LastUpdatedById = bugDbModel.CreatorId;

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

            var sortingOptions = sortingFactory.CreateSortingOptions(sortOptions);

            var pageInfo = PagingInfo.CreatePage(page, pageSize);

            var bugList = await bugRepository.RetrieveData(filters, sortingOptions, searchTerm, pageInfo);

            return new PagedList<BugModel>
            {
                CurrentPage = page,
                PageSize = pageSize,
                Items = mapper.Map<List<BugModel>>(bugList),
                TotalCount = await bugRepository.CountTotal()
            };
        }

        public async Task<BugModel> UpdateBug(EditBugViewModel model)
        {
            var bug = await bugRepository.GetById(model.Id);

            if (bug != null)
            {
                mapper.Map(model, bug);

                bug = await bugRepository.Update(bug);
            }
            else
            {
                bug = await bugRepository.Create(mapper.Map<Bug>(model));
            }

            return mapper.Map<BugModel>(bug);
        }
    }
}

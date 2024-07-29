using AutoMapper;
using Core.DTOs;
using Core.DTOs.Bugs;
using Core.Repository.BugRepo;
using Core.Utilities;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public class BugService : IBugService
    {
        private readonly IBugRepository _bugRepository;
        private readonly IBugFilterFactory _filterFactory;
        private readonly IBugSortingOptionsFactory _sortingFactory;
        private readonly IMapper _mapper;

        public BugService(IBugRepository bugRepository, IBugFilterFactory filterFactory,
            IBugSortingOptionsFactory sortingFactory, IMapper mapper)
        {
            _bugRepository = bugRepository;
            _filterFactory = filterFactory;
            _sortingFactory = sortingFactory;
            _mapper = mapper;
        }

        public async Task<BugModel> CreateBug(AddBugModel model)
        {
            var bugDbModel = _mapper.Map<Bug>(model);

            bugDbModel.LastUpdatedOn = bugDbModel.CreatedOn;
            bugDbModel.LastUpdatedById = bugDbModel.CreatorId;

            var newBug = await _bugRepository.Create(bugDbModel);

            return _mapper.Map<BugModel>(newBug);
        }

        public async Task DeleteBug(int id)
        {
            await _bugRepository.DeleteById(id);
        }

        public async Task<bool> DoesExist(int id)
            => await _bugRepository.DoesExist(id);

        public async Task<BugModel?> GetBugById(int id)
        {
            var bug = await _bugRepository.GetById(id);

            if (bug != null)
            {
                return _mapper.Map<BugModel>(bug);
            }

            return null;
        }

        public async Task<PagedList<BugModel>> GetBugs(int page, int pageSize, string? searchTerm, string? sortOptions, string? filter)
        {
            var filters = await _filterFactory.CreateFilters(filter);

            var sortingOptions = _sortingFactory.CreateSortingOptions(sortOptions);

            int totalElements = await _bugRepository.CountTotal();

            var pageInfo = PagingInfo.CreatePage(page, pageSize, totalElements);

            var bugList = await _bugRepository.RetrieveData(filters, sortingOptions, searchTerm, pageInfo);

            return new PagedList<BugModel>
            {
                PageInfo = pageInfo,
                Items = _mapper.Map<List<BugModel>>(bugList),
            };
        }

        public Task<PagedList<BugModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<BugModel>> GetAll(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<BugModel>> GetAssignedToUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<BugModel>> GetCreatedByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<BugModel>> GetUnassigned()
        {
            throw new NotImplementedException();
        }

        public async Task<BugModel> UpdateBug(EditBugViewModel model)
        {
            var bug = await _bugRepository.GetById(model.Id);

            if (bug != null)
            {
                _mapper.Map(model, bug);

                bug = await _bugRepository.Update(bug);
            }
            else
            {
                bug = await _bugRepository.Create(_mapper.Map<Bug>(model));
            }

            return _mapper.Map<BugModel>(bug);
        }
    }
}

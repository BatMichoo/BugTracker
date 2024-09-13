using AutoMapper;
using Core.BaseService;
using Core.DTOs.Bugs;
using Core.QueryParameters;
using Core.Repository.BugRepo;
using Core.Utilities;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public class BugService : AdvancedService<Bug, BugOrderBy, BugFilterType, BugModel, AddBugModel, EditBugModel>, IBugService
    {
        public BugService(IBugRepository bugRepository, IBugFilterFactory filterFactory,
            IBugSortingOptionsFactory sortingFactory, IMapper mapper) : base(bugRepository, mapper, sortingFactory, filterFactory)
        {
        }        

        public async Task<bool> DoesExist(int id)
            => await _repository.DoesExist(id);

        public async Task<List<BugModel>> GetAllBetweenTwoDates(DateTime startDate, DateTime endDate)
        {
            var startDateFilter = _filterFactory.CreateFilter(BugFilterType.CreatedOn, $"{startDate};>=");
            var endDateFilter = _filterFactory.CreateFilter(BugFilterType.CreatedOn, $"{endDate};<=");

            var filtersList = new List<IFilter<Bug>> { startDateFilter, endDateFilter };

            var queryParameters = new QueryParameters<Bug>(filtersList);

            var allBugsInPeriod = await _advancedRepository.RunQuery(queryParameters);

            return _mapper.Map<List<BugModel>>(allBugsInPeriod);
        }

        public async Task<List<BugModel>> GetAssignedToUserId(string userId)
        {
            QueryParameters<Bug> queryParameters = NewMethod(userId);

            var bugs = await _advancedRepository.RunQuery(queryParameters);

            return _mapper.Map<List<BugModel>>(bugs);
        }

        private QueryParameters<Bug> NewMethod(string userId)
        {
            var filter = _filterFactory.CreateFilter(BugFilterType.AssignedTo, userId);

            var filterList = new List<IFilter<Bug>> { filter };

            var queryParameters = new QueryParameters<Bug>(filterList);
            return queryParameters;
        }

        public async Task<List<BugModel>> GetCreatedByUserId(string userId)
        {
            var filter = _filterFactory.CreateFilter(BugFilterType.CreatedBy, userId);

            var filterList = new List<IFilter<Bug>> { filter };

            var queryParameters = new QueryParameters<Bug>(filterList);

            var bugs = await _advancedRepository.RunQuery(queryParameters);

            return _mapper.Map<List<BugModel>>(bugs);
        }

        public async Task<List<BugModel>> GetUnassigned()
        {
            var filter = _filterFactory.CreateFilter(BugFilterType.AssignedTo);

            var filterList = new List<IFilter<Bug>> { filter };

            var queryParameters = new QueryParameters<Bug>(filterList);

            var bugs = await _advancedRepository.RunQuery(queryParameters);

            return _mapper.Map<List<BugModel>>(bugs);
        }        
    }
}

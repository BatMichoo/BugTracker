using AutoMapper;
using Core.BaseService;
using Core.DTOs;
using Core.DTOs.Bugs;
using Core.QueryParameters;
using Core.Repository.BugRepo;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.BugService
{
    public class BugService : AdvancedService<Bug, BugSortBy, BugFilterType, BugModel, AddBugModel, EditBugModel>, IBugService
    {
        private IBugQueryFactory QueryFactory => (IBugQueryFactory) _queryFactory;

        public BugService(IBugRepository repository, IMapper mapper, 
            IBugQueryFactory queryFactory) 
            : base(repository, mapper, queryFactory)
        {
        }

        public async Task<bool> DoesExist(int id)
            => await AdvancedRepository.DoesExist(id);

        public async Task<PagedList<BugModel>> GetAllBetweenTwoDates(DateTime startDate, DateTime endDate)
        {
            var queryParameters = QueryFactory.CreateBetweenTwoDatesQuery(startDate, endDate);

            var allBugsInPeriod = await AdvancedRepository.RunQuery(queryParameters);

            return new PagedList<BugModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<BugModel>>(allBugsInPeriod)
            };
        }       

        public async Task<PagedList<BugModel>> GetAssignedToUserId(string userId)
        {
            var queryParameters = QueryFactory.CreateAssignedToUserQuery(userId);

            var bugs = await AdvancedRepository.RunQuery(queryParameters);

            return new PagedList<BugModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<BugModel>>(bugs)
            };
        }        

        public async Task<PagedList<BugModel>> GetCreatedByUserId(string userId)
        {
            var queryParameters = QueryFactory.CreateMadeByUserQuery(userId);

            var bugs = await AdvancedRepository.RunQuery(queryParameters);

            return new PagedList<BugModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<BugModel>>(bugs)
            };
        }

        public async Task<PagedList<BugModel>> GetUnassigned()
        {
            var queryParameters = QueryFactory.CreateNotAssignedQuery();

            var bugs = await AdvancedRepository.RunQuery(queryParameters);

            return new PagedList<BugModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<BugModel>>(bugs)
            };
        }        
    }
}

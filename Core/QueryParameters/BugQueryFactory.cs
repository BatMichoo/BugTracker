using Core.Utilities;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.QueryParameters
{
    public sealed class BugQueryFactory : QueryFactory<Bug, BugSortBy, BugFilterType>, IBugQueryFactory
    {
        public BugQueryFactory(IBugSortingOptionsFactory sortingOptionsFactory, IBugFilterFactory filterFactory) 
            : base(sortingOptionsFactory, filterFactory)
        {
        }

        public QueryParameters<Bug> CreateAssignedToUserQuery(string userId)
        {
            var filter = _filterFactory.CreateFilter(BugFilterType.AssignedTo, userId);

            var filterList = new List<IFilter<Bug>> { filter };

            var sortingOptions = _sortingOptionsFactory.CreateSortingOptions();

            return new QueryParameters<Bug>(filters: filterList, sortOptions: sortingOptions);
        }

        public QueryParameters<Bug> CreateBetweenTwoDatesQuery(DateTime startDate, DateTime endDate)
        {
            var startDateFilter = _filterFactory.CreateFilter(BugFilterType.CreatedOn, $"{startDate};>=");
            var endDateFilter = _filterFactory.CreateFilter(BugFilterType.CreatedOn, $"{endDate};<=");

            var filtersList = new List<IFilter<Bug>> { startDateFilter, endDateFilter };

            return new QueryParameters<Bug>(filtersList); 
        }

        public QueryParameters<Bug> CreateMadeByUserQuery(string userId)
        {
            var filter = _filterFactory.CreateFilter(BugFilterType.CreatedBy, userId);

            var filterList = new List<IFilter<Bug>> { filter };

            return new QueryParameters<Bug>(filterList);
        }

        public QueryParameters<Bug> CreateNotAssignedQuery()
        {
            var filter = _filterFactory.CreateFilter(BugFilterType.AssignedTo);

            var filterList = new List<IFilter<Bug>> { filter };

            return new QueryParameters<Bug>(filterList);
        }
    }
}

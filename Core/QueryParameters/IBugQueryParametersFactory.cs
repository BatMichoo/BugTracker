using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;

namespace Core.QueryParameters
{
    public interface IBugQueryParametersFactory : IQueryParametersFactory<Bug, BugSortBy, BugFilterType>
    {
        public QueryParameters<Bug> CreateAssignedToUserQuery(string userId);
        public QueryParameters<Bug> CreateBetweenTwoDatesQuery(DateTime startDate, DateTime endDate);
        public QueryParameters<Bug> CreateMadeByUserQuery(string userId);
        public QueryParameters<Bug> CreateNotAssignedQuery();
    }
}

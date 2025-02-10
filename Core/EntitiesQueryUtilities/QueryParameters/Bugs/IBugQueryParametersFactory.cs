using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.Bugs;

namespace Core.EntitiesQueryUtilities.QueryParameters.Bugs
{
    public interface IBugQueryParametersFactory : IQueryParametersFactory<Bug, BugSortBy, BugFilterType>
    {
        public QueryParameters<Bug> CreateAssignedToUserQuery(string userId);
        public QueryParameters<Bug> CreateBetweenTwoDatesQuery(DateTime startDate, DateTime endDate);
        public QueryParameters<Bug> CreateMadeByUserQuery(string userId);
        public QueryParameters<Bug> CreateNotAssignedQuery();
    }
}

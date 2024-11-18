using Core.EntitiesQueryUtilities.Bugs;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;

namespace UnitTests.QueryParametersFactory
{
    public class TestQueryParamsFactory : BugQueryParametersFactory
    {
        public TestQueryParamsFactory(IBugSortingOptionsFactory sortingOptionsFactory, IBugFilterFactory filterFactory)
            : base(sortingOptionsFactory, filterFactory)
        {
        }
    }
}

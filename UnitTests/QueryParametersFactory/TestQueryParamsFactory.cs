using Core.QueryParameters.Bug;
using Core.Utilities.Bugs;

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

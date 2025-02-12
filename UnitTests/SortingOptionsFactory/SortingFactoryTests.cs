using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.Bugs;

namespace UnitTests.SortingOptionsFactory
{
    public class SortingFactoryTests
    {
        private TestSortingFactory? _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new TestSortingFactory();
        }

        [TearDown]
        public void TearDown()
        {
            _factory = null;
        }

        [Test]
        public void Test_Create_Returns_SingleByIdDesc()
        {
            var expected = new BugSortingOptions(SortOrder.Descending, BugSortBy.Id);
            
            string sortingOptionsInput = $"Id{FilterQuerySeparators.KeyValue}Descending";

            var result = _factory!.CreateSortingOptions(sortingOptionsInput);

            Assert.That(result, Has.Count.EqualTo(1));
            
            var option = (BugSortingOptions) result[0];

            Assert.That(option.SortBy, Is.EqualTo(expected.SortBy));
            Assert.That(option.SortOrder, Is.EqualTo(expected.SortOrder));
        }

        [Test]
        public void Test_Create_Returns_TwoByIdDesc_CreatedOn_Asc()
        {
            var expected = new List<BugSortingOptions>
            {
                new BugSortingOptions(SortOrder.Descending, BugSortBy.Id),
                new BugSortingOptions(SortOrder.Ascending, BugSortBy.CreatedOn)
            };

            string sortingOptionsInput = $"Id{FilterQuerySeparators.KeyValue}Descending{FilterQuerySeparators.Filter}CreatedOn{FilterQuerySeparators.KeyValue}Ascending";

            var results = _factory!.CreateSortingOptions(sortingOptionsInput);

            Assert.That(results, Has.Count.EqualTo(2));

            for (int i = 0; i < results.Count; i++)
            {
                var result = (BugSortingOptions) results[i];

                Assert.That(result.SortBy, Is.EqualTo(expected[i].SortBy));
                Assert.That(result.SortOrder, Is.EqualTo(expected[i].SortOrder));
            }
        }
    }
}

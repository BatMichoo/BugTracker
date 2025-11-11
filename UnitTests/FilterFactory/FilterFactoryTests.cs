using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.Bugs;
using Core.EntitiesQueryUtilities.Bugs.Filters;

namespace UnitTests.FilterFactory
{
    public class FilterFactoryTests
    {
        private IBugFilterFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new BugFilterFactory();
        }

        [Test]
        public void CreateFilter_Returns_AssignedToFilter()
        {
            string filterInput = $"assignedTo{FilterQuerySeparators.KeyValue}abc";

            var filter = _factory.CreateFilters(filterInput).First();

            Assert.That(filter.GetType(), Is.EqualTo(typeof(BugAssignedToFilter)));
        }

        [Test]
        public void CreateFilter_Returns_CreatedByFilter()
        {
            string filterInput = $"createdBy{FilterQuerySeparators.KeyValue}abc";

            var filter = _factory.CreateFilters(filterInput).First();

            Assert.That(filter.GetType(), Is.EqualTo(typeof(BugCreatedByFilter)));
        }

        [Test]
        public void CreateFilter_Returns_CreatedOnFilter()
        {
            string filterInput = $"createdOn{FilterQuerySeparators.KeyValue}01.11.2024{FilterQuerySeparators.KeyValue}>=";

            var filter = _factory.CreateFilters(filterInput).First();

            Assert.That(filter.GetType(), Is.EqualTo(typeof(BugCreatedOnFilter)));
        }

        [Test]
        public void CreateFilter_Returns_PriorityFilter()
        {
            string filterInput = $"priority{FilterQuerySeparators.KeyValue}0";

            var filter = _factory.CreateFilters(filterInput).First();

            Assert.That(filter.GetType(), Is.EqualTo(typeof(BugPriorityFilter)));
        }

        [Test]
        public void CreateFilter_Returns_StatusFilter()
        {
            string filterInput = $"status{FilterQuerySeparators.KeyValue}0";

            var filter = _factory.CreateFilters(filterInput).First();

            Assert.That(filter.GetType(), Is.EqualTo(typeof(BugStatusFilter)));
        }
    }
}

using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.Bugs;

namespace UnitTests.Filters
{
    public class BugFilterTests
    {
        private IBugFilterFactory _factory;
        private readonly List<Bug> _bugs = new List<Bug>
        {
            new Bug { Id = 1, AssigneeId = "abc", CreatorId = "a", Description = "test 1234", Priority = 4,
                Status = 0, LastUpdatedById = "a", CreatedOn = DateTime.Parse("01.11.2024") },
            new Bug { Id = 2, AssigneeId = "abcd", CreatorId = "ab", Description = "test 12345", Priority = 3,
                Status = 1, LastUpdatedById = "ab", CreatedOn = DateTime.Parse("05.11.2024") },
            new Bug { Id = 3, AssigneeId = "abcd", CreatorId = "abc", Description = "test 123457", Priority = 3,
                Status = 1, LastUpdatedById = "ab", CreatedOn = DateTime.Parse("10.11.2024") },
            new Bug { Id = 4, AssigneeId = null, CreatorId = "abc", Description = "test 123457", Priority = 3,
                Status = 1, LastUpdatedById = "ab", CreatedOn = DateTime.Parse("10.11.2024 22:15:00") }
        };

        [SetUp]
        public void SetUp()
        {
            _factory = new BugFilterFactory();
        }

        private List<Bug> CreateAndApplyFilter(string filterInput)
        {
            var filterList = _factory.CreateFilters(filterInput);

            Assert.That(filterList, Has.Count.EqualTo(1));

            var filter = filterList[0];

            var filterFunc = filter.Apply().Compile();

            var filteredBugs = _bugs.Where(b => filterFunc(b)).ToList();

            return filteredBugs;
        }

        [Test]
        public void CreateFilter_Returns_1_AssignedToFilter()
        {
            string filterInput = $"assignedTo{FilterQuerySeparators.KeyValue}abc";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            Assert.That(filteredBugs, Has.Count.EqualTo(1));
            Assert.That(filteredBugs[0].AssigneeId, Is.EqualTo("abc"));
        }

        [Test]
        public void CreateFilter_Returns_0_AssignedToFilter()
        {
            string filterInput = "assignedTo";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            Assert.That(filteredBugs, Has.Count.EqualTo(0));
        }

        [Test]
        public void CreateFilter_Returns_2_AssignedToFilter()
        {
            string filterInput = $"assignedTo{FilterQuerySeparators.KeyValue}abcd";

            List<Bug> filteredBugs = CreateAndApplyFilter(filterInput);

            Assert.That(filteredBugs, Has.Count.EqualTo(2));

            foreach (var b in filteredBugs)
            {
                Assert.That(b.AssigneeId, Is.EqualTo("abcd"));
            }
        }

        [Test]
        public void CreateFilter_Returns_2_CreatedByFilter()
        {
            string filterInput = $"createdBy{FilterQuerySeparators.KeyValue}abc";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            Assert.That(filteredBugs, Has.Count.EqualTo(2));
            Assert.That(filteredBugs[0].CreatorId, Is.EqualTo("abc"));
        }


        [Test]
        public void CreateFilter_Returns_4_CreatedOnFilter()
        {
            string filterInput = $"createdOn{FilterQuerySeparators.KeyValue}01.11.2024{FilterQuerySeparators.KeyValue}>=";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            int expectedCount = 4;

            Assert.That(filteredBugs, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public void CreateFilter_Returns_2_CreatedOnFilter()
        {
            string filterInput = $"createdOn{FilterQuerySeparators.KeyValue}01.11.2024{FilterQuerySeparators.KeyValue}>";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            int expectedCount = 3;

            Assert.That(filteredBugs, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public void CreateFilter_Returns_2_CreatedOnFilter_Equals()
        {
            string filterInput = $"createdOn{FilterQuerySeparators.KeyValue}10.11.2024";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            int expectedCount = 2;

            Assert.That(filteredBugs, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public void CreateFilter_Returns_3_PriorityFilter()
        {
            string filterInput = $"priority{FilterQuerySeparators.KeyValue}3";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            int expectedCount = 3;

            Assert.That(filteredBugs, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public void CreateFilter_Returns_1_StatusFilter()
        {
            string filterInput = $"status{FilterQuerySeparators.KeyValue}0";

            var filteredBugs = CreateAndApplyFilter(filterInput);

            int expectedCount = 1;

            Assert.That(filteredBugs, Has.Count.EqualTo(expectedCount));
        }
    }
}

using Core.EntitiesQueryUtilities.Bugs;
using Infrastructure;
using Infrastructure.QueryBuilders;
using UnitTests.Repository;
using UnitTests.Utilities;

namespace UnitTests.QueryParametersFactory
{
    public class QueryParamsFactoryTests
    {
        private readonly TestQueryParamsFactory _factory;
        private TrackerDbContext _dbContext;
        private TestRepository _repository;

        public QueryParamsFactoryTests()
        {
            _factory = new TestQueryParamsFactory(new BugSortingOptionsFactory(), new BugFilterFactory());
        }

        [SetUp]
        public void SetUp()
        {
            _dbContext = Database.Initializer.TestDatabase();

            Database.Seeder.WithBugs(_dbContext);

            _repository = new TestRepository(_dbContext, new BugQueryableBuilder());
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _repository = null;
        }

        [Test]
        public async Task Test_AssignedToUserQueryParams()
        {
            string userId = "abc";

            var queryParameters = _factory.CreateAssignedToUserQuery(userId);
            var result = await _repository.ExecuteQuery(queryParameters);

            Assert.That(queryParameters.Filters.Count, Is.EqualTo(1));
            Assert.That(result.Count, Is.AtLeast(1));
            Assert.That(result.All(e => e.AssigneeId == userId), Is.True);
        }

        [Test]
        public async Task Test_CreatedByUserQueryParams()
        {
            string userId = "abc";

            var queryParameters = _factory.CreateMadeByUserQuery(userId);
            var result = await _repository.ExecuteQuery(queryParameters);

            Assert.That(queryParameters.Filters.Count, Is.EqualTo(1));
            Assert.That(result.Count, Is.AtLeast(1));
            Assert.That(result.All(e => e.CreatorId == userId), Is.True);
        }

        [Test]
        public async Task Test_GetAllQueryParams()
        {
            var queryParameters = _factory.CreateGetAllQuery();

            var result = await _repository.ExecuteQuery(queryParameters);

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task BetweenTwoDates_ReturnsOneResult()
        {
            var startDate = DateTime.Parse("1.10.2024");
            var endDate = DateTime.Parse("2.10.2024");
            var queryParams = _factory.CreateBetweenTwoDatesQuery(startDate, endDate);

            var result = await _repository.ExecuteQuery(queryParams);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task NotAssignedQuery_ReturnsNoResults()
        {
            var queryParams = _factory.CreateNotAssignedQuery();

            var result = await _repository.ExecuteQuery(queryParams);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}

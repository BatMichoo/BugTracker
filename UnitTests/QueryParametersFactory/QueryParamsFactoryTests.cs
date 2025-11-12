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
            var user = _dbContext.Users.FirstOrDefault()!;
            string userId = user.Id;

            var queryParameters = _factory.CreateAssignedToUserQuery(userId);
            var result = await _repository.ExecuteQuery(queryParameters);
            Assert.Multiple(() =>
            {
                Assert.That(queryParameters.Filters, Has.Count.EqualTo(1));
                Assert.That(result, Has.Count.AtLeast(1));
            });
            Assert.That(result.All(e => e.AssigneeId == userId), Is.True);
        }

        [Test]
        public async Task Test_CreatedByUserQueryParams()
        {
            var user = _dbContext.Users.FirstOrDefault()!;
            string userId = user.Id;

            var queryParameters = _factory.CreateMadeByUserQuery(userId);
            var result = await _repository.ExecuteQuery(queryParameters);
            Assert.Multiple(() =>
            {
                Assert.That(queryParameters.Filters, Has.Count.EqualTo(1));
                Assert.That(result, Has.Count.AtLeast(1));
            });
            Assert.That(result.All(e => e.CreatorId == userId), Is.True);
        }

        [Test]
        public async Task Test_GetAllQueryParams()
        {
            var queryParameters = _factory.CreateGetAllQuery();

            var result = await _repository.ExecuteQuery(queryParameters);

            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task BetweenTwoDates_ReturnsOneResult()
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now.AddMonths(1);
            var queryParams = _factory.CreateBetweenTwoDatesQuery(startDate, endDate);

            var result = await _repository.ExecuteQuery(queryParams);

            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task NotAssignedQuery_ReturnsNoResult()
        {
            var queryParams = _factory.CreateNotAssignedQuery();

            var result = await _repository.ExecuteQuery(queryParams);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}

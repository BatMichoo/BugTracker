using Core.QueryBuilders;
using Core.Utilities.Bugs;
using Infrastructure.Models.BugEntity;
using Infrastructure.Models.UserEntity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using UnitTests.Repository;

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
            var options = new DbContextOptionsBuilder<TrackerDbContext>()
            .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new TrackerDbContext(options);

            SeedInMemoryDatabase();

            _repository = new TestRepository(_dbContext, new BugQueryableBuilder());
        }

        private void SeedInMemoryDatabase()
        {
            var user = new BugUser { Id = "abc", UserName = "tester1" };
            var user2 = new BugUser { Id = "a", UserName = "tester2" };
            var user3 = new BugUser { Id = "ab", UserName = "tester3" };

            var entity = new Bug { Id = 1, AssigneeId = "abc", CreatorId = "a", Description = "test 1234", Priority = 4, Status = 0, LastUpdatedById = "a", CreatedOn = DateTime.Parse("1.10.2024") };
            var entity2 = new Bug { Id = 2, AssigneeId = "abcd", CreatorId = "ab", Description = "test 12345", Priority = 3, Status = 1, LastUpdatedById = "ab", CreatedOn = DateTime.Parse("30.9.2024") };
            var entity3 = new Bug { Id = 3, AssigneeId = "abcd", CreatorId = "abc", Description = "test 123457", Priority = 3, Status = 1, LastUpdatedById = "ab", CreatedOn = DateTime.Parse("5.10.2024") };

            _dbContext.Bugs.AddRange(new List<Bug> { entity, entity2, entity3 });
            _dbContext.Users.AddRange(new List<BugUser> { user, user2, user3 });

            _dbContext.SaveChanges();
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

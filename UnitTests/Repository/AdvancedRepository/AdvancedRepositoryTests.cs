using Core.QueryParameters;
using Core.Utilities.Bugs;
using Infrastructure;
using Infrastructure.Models.BugEntity;
using Infrastructure.Models.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Repository.AdvancedRepository
{
    public class AdvancedRepositoryTests
    {
        private AdvancedTestRepository? _repository;
        private TrackerDbContext _dbContext;
        private readonly IBugQueryFactory _queryFactory;

        public AdvancedRepositoryTests()
        {
            _queryFactory = new BugQueryFactory(new BugSortingOptionsFactory(), new BugFilterFactory());
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TrackerDbContext>()
            .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new TrackerDbContext(options);

            _dbContext.Database.EnsureCreated();

            var user = new BugUser { Id = "abc", UserName = "tester1" };
            var user2 = new BugUser { Id = "a", UserName = "tester2" };
            var user3 = new BugUser { Id = "ab", UserName = "tester3" };

            var entity = new Bug { Id = 1, AssigneeId = "abc", CreatorId = "a", Description = "test 1234", Priority = 4, Status = 0, LastUpdatedById = "a" };
            var entity2 = new Bug { Id = 2, AssigneeId = "abcd", CreatorId = "ab", Description = "test 12345", Priority = 3, Status = 1, LastUpdatedById = "ab" };

            _dbContext.Bugs.Add(entity);
            _dbContext.Bugs.Add(entity2);
            _dbContext.Users.AddRange(new List<BugUser> { user, user2, user3 });

            _dbContext.SaveChanges();

            _repository = new AdvancedTestRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _repository = null;
        }

        [Test]
        public async Task RunQuery_AppliesAssignedToUserQueryFilters()
        {
            string userId = "abc";
            var queryParameters = _queryFactory.CreateAssignedToUserQuery(userId);


            var result = await _repository!.RunQuery(queryParameters);


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.All(r => r.AssigneeId == userId), Is.True);
        }
    }
}

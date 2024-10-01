using Core.QueryBuilders;
using Core.QueryParameters;
using Core.Utilities.Bugs;
using Infrastructure;
using Infrastructure.Models.BugEntity;
using Infrastructure.Models.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Repository
{
    public class RepositoryTests
    {
        private TestRepository? _repository;
        private TrackerDbContext _dbContext;
        private readonly IBugQueryParametersFactory _paramsFactory;

        public RepositoryTests()
        {
            _paramsFactory = new BugQueryParametersFactory(new BugSortingOptionsFactory(), new BugFilterFactory());
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

            var entity = new Bug { Id = 1, AssigneeId = "abc", CreatorId = "a", Description = "test 1234", Priority = 4, Status = 0, LastUpdatedById = "a" };
            var entity2 = new Bug { Id = 2, AssigneeId = "abcd", CreatorId = "ab", Description = "test 12345", Priority = 3, Status = 1, LastUpdatedById = "ab" };
            var entity3 = new Bug { Id = 3, AssigneeId = "abcd", CreatorId = "abc", Description = "test 123457", Priority = 3, Status = 1, LastUpdatedById = "ab" };

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
        public async Task Create_ShouldReturnCreatedItem()
        {
            var entity = new Bug { Id = 4, AssigneeId = "abcd", CreatorId = "ab", Description = "test 123456", Priority = 2, Status = 2, LastUpdatedById = "ab" };

            var result = await _repository!.Create(entity);

            Assert.That(result.Id, Is.EqualTo(entity.Id));
        }

        [Test]
        public async Task GetById_ReturnsEntityById()
        {
            var idToGet = 1;

            var result = await _repository!.GetById(idToGet);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(idToGet));
        }

        [Test]
        public async Task DeleteById_DeletesTheEntityWithId()
        {
            int idToDelete = 1;

            await _repository!.DeleteById(idToDelete);

            var result = await _repository.GetById(idToDelete);

            Assert.That(result is null, Is.True);
        }

        [Test]
        public async Task Update()
        {
            var updatedEntity = new Bug { Id = 1, AssigneeId = "ab", CreatorId = "abc", Description = "test 1234 update", Priority = 2, Status = 3, LastUpdatedById = "b" };

            var result = await _repository!.Update(updatedEntity);

            Assert.That(result.Id!, Is.EqualTo(updatedEntity.Id));
            Assert.That(result.AssigneeId!, Is.EqualTo(updatedEntity.AssigneeId));
            Assert.That(result.CreatorId!, Is.EqualTo(updatedEntity.CreatorId));
            Assert.That(result.Description!, Is.EqualTo(updatedEntity.Description));
            Assert.That(result.Priority, Is.EqualTo(updatedEntity.Priority));
            Assert.That(result.Status, Is.EqualTo(updatedEntity.Status));
            Assert.That(result.LastUpdatedById, Is.EqualTo(updatedEntity.LastUpdatedById));
        }

        [Test]
        public async Task GetAll_ReturnsNonEmptyCollection()
        {
            int mininumTreshold = 1;
            var queryParams = await _paramsFactory.CreateGetAllQuery();

            var result = await _repository!.ExecuteQuery(queryParams);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(mininumTreshold));
        }

        [Test]
        public async Task TotalCount_Returns3()
        {
            int expectedResult = 3;

            var result = await _repository!.CountTotal();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task DoesExists_ReturnsTrue()
        {
            int idToCheck = 1;

            var result = await _repository!.DoesExist(idToCheck);

            Assert.That(result, Is.True);
        }
    }
}

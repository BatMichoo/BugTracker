using Core.Entities.BugEntity;
using Core.Entities.UserEntity;
using Core.EntitiesQueryUtilities.Bugs;
using Core.EntitiesQueryUtilities.QueryBuilders.Bugs;
using Core.EntitiesQueryUtilities.QueryParameters;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;
using Infrastructure;
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
            var user = new BugUser { Id = "abc", UserName = "tester1", Name = "Pesho" };
            var user2 = new BugUser { Id = "a", UserName = "tester2", Name = "Gosho" };
            var user3 = new BugUser { Id = "ab", UserName = "tester3", Name = "Sasho" };

            var entity = new Bug { Id = 1, Title = "Title1", AssigneeId = "abc", CreatorId = "a", Description = "test 1234", Priority = 4, Status = 0, LastUpdatedById = "a" };
            var entity2 = new Bug { Id = 2, Title = "Title2", AssigneeId = "abcd", CreatorId = "ab", Description = "test 12345", Priority = 3, Status = 1, LastUpdatedById = "ab" };
            var entity3 = new Bug { Id = 3, Title = "Title3", AssigneeId = "abcd", CreatorId = "abc", Description = "test 123457", Priority = 3, Status = 1, LastUpdatedById = "ab" };

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
            var entity = new Bug { Id = 4, Title = "Title4", AssigneeId = "abcd", CreatorId = "ab", Description = "test 123456", Priority = 2, Status = 2, LastUpdatedById = "ab" };

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
            int idToDelete = 3;

            await _repository!.DeleteById(idToDelete);

            var result = await _repository.GetById(idToDelete);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Update()
        {
            var updatedEntity = new Bug { Id = 1, Title = "Title5", AssigneeId = "ab", CreatorId = "abc", Description = "test 1234 update", Priority = 2, Status = 3, LastUpdatedById = "b" };

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
            var queryParams = _paramsFactory.CreateGetAllQuery();

            var result = await _repository!.ExecuteQuery(queryParams);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.AtLeast(mininumTreshold));
        }

        [Test]
        public async Task TotalCount_Returns3()
        {
            int expectedResult = 3;

            var result = await _repository!.Count(new QueryParameters<Bug>());

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task DoesExists_ReturnsTrue()
        {
            int idToCheck = 1;

            var result = await _repository!.DoesExist(idToCheck);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteByEntity_DeletesSaidEntity()
        {
            int idToGet = 2;
            var entity = await _repository!.GetById(idToGet);

            await _repository.Delete(entity!);

            var result = await _repository.DoesExist(idToGet);

            Assert.That(result, Is.False);
        }
    }
}

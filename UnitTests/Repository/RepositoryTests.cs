using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.QueryParameters;
using Infrastructure;
using Infrastructure.QueryBuilders;
using UnitTests.Utilities;

namespace UnitTests.Repository
{
    public class RepositoryTests
    {
        private TestRepository? _repository;
        private TrackerDbContext _dbContext;

        public RepositoryTests()
        {
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
        public async Task Create_ShouldReturnCreatedItem()
        {
            var user = _dbContext.Users.FirstOrDefault();
            var entity = new Bug { Title = "Title4", AssigneeId = user.Id, CreatorId = "ab", Description = "test 123456", Priority = 2, Status = 2, LastUpdatedById = user.Id, Creator = user };

            var result = await _repository!.Create(entity);

            Assert.That(result.Title, Is.EqualTo(entity.Title));
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
            var user = _dbContext.Users.FirstOrDefault();
            var bug = _dbContext.Bugs.FirstOrDefault();
            var updatedEntity = new Bug { Id = bug.Id, Title = "Title5", AssigneeId = user.Id , Description = "test 1234 update", Priority = 2, Status = 3, LastUpdatedById = user.Id};

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

            var result = await _repository!.GetAll();

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

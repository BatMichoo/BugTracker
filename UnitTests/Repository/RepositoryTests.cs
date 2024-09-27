using Core.Models.Bugs.BugEnums;
using Infrastructure;
using Infrastructure.Models.BugEntity;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Repository
{
    public class RepositoryTests
    {
        private SimpleTestRepository? _repository;
        private TrackerDbContext _dbContext;

        public RepositoryTests()
        {            
        }

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TrackerDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new TrackerDbContext(options);

            var entity = new Bug { Id = 1, AssigneeId = "abc", CreatorId = "a", Description = "test 1234", Priority = 4, Status = 0, LastUpdatedById = "a" };
            var entity2 = new Bug { Id = 2, AssigneeId = "abcd", CreatorId = "ab", Description = "test 12345", Priority = 3, Status = 1, LastUpdatedById = "ab" };

            _dbContext.Add(entity);
            _dbContext.Add(entity2);

            _dbContext.SaveChanges();

            _repository = new SimpleTestRepository(_dbContext);
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
            var entity = new Bug { Id = 3, AssigneeId = "abcd", CreatorId = "ad", Description = "test 123456", Priority = 2, Status = 2, LastUpdatedById = "ad"};

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
            var updatedEntity = new Bug { Id = 1, AssigneeId = "updated ID", CreatorId = "updated", Description = "test 1234 update", Priority = 2, Status = 3, LastUpdatedById = "b" };

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
            var result = await _repository!.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.AtLeast(1));
        }
    }
}

using Core.DTOs.Bugs;
using Core.Entities.BugEntity;
using Core.EntitiesQueryUtilities.QueryParameters;
using Core.Models.Bugs.BugEnums;
using Infrastructure;
using Infrastructure.QueryBuilders;
using UnitTests.Repository;
using UnitTests.Utilities;

namespace UnitTests.Service
{
    public class ServiceTests
    {
        private TestService? _service;
        private TrackerDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = Database.Initializer.TestDatabaseWithBugSeeding();
            var mapper = Mappings.CreateMapper();
            var repository = new TestRepository(_context, new BugQueryableBuilder());

            _service = new TestService(repository, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _service = null;
        }

        [Test]
        public async Task Create_ShouldReturnCreatedItem()
        {
            var user = _context.Users.FirstOrDefault();
            var entity = new AddBugModel { Title = "Title4", CreatorId = user.Id, Description = "test 123456", Priority = BugPriority.Normal, Status = BugStatus.InProgress };

            var result = await _service!.Create(entity);

            Assert.That(result, Is.TypeOf<BugModel>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Title, Is.EqualTo(entity.Title));
                Assert.That(result.CreatedBy.Id, Is.EqualTo(entity.CreatorId));
            });
        }

        [Test]
        public async Task GetById_ReturnsEntityById()
        {
            var idToGet = 1;

            var result = await _service!.GetById(idToGet, isFullyIncluded: true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(idToGet));
        }

        [Test]
        public async Task DeleteById_DeletesTheEntityWithId()
        {
            int idToDelete = 3;

            await _service!.Delete(idToDelete);

            var result = await _service.GetById(idToDelete, isFullyIncluded: false);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Update()
        {
            var user = _context.Users.FirstOrDefault();

            var updatedEntity = new EditBugModel { Id = 1, Title = "Test Title", AssigneeId = user.Id, Description = "test 1234 update", Priority = BugPriority.Normal, Status = BugStatus.Fixed, LastUpdatedById = user.Id };

            var result = await _service!.Update(updatedEntity);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id!, Is.EqualTo(updatedEntity.Id));
                Assert.That(result.AssignedTo!.Id!, Is.EqualTo(updatedEntity.AssigneeId));
                Assert.That(result.Description!, Is.EqualTo(updatedEntity.Description));
                Assert.That(result.Priority, Is.EqualTo(updatedEntity.Priority));
                Assert.That(result.Status, Is.EqualTo(updatedEntity.Status));
                Assert.That(result.LastUpdatedBy.Id, Is.EqualTo(updatedEntity.LastUpdatedById));
            });
        }

        [Test]
        public async Task GetAll_ReturnsNonEmptyCollection()
        {
            int mininumTreshold = 1;

            var result = await _service!.Fetch(new QueryParameters<Bug>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items, Has.Count.AtLeast(mininumTreshold));
        }

        [Test]
        public async Task TotalCount_Returns3()
        {
            int expectedResult = 3;

            var result = await _service!.Fetch(new QueryParameters<Bug>());

            Assert.That(result.Items, Has.Count.EqualTo(expectedResult));
        }

        [Test]
        public async Task DoesExists_ReturnsTrue()
        {
            int idToCheck = 1;

            var result = await _service!.DoesExist(idToCheck);

            Assert.That(result, Is.True);
        }        
    }
}

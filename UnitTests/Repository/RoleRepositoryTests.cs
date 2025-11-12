using Core.Entities.CustomRole;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Utilities;

namespace UnitTests.Repository
{
    [TestFixture]
    public class RoleRepositoryTests
    {
        private RoleRepository? _repository;
        private TrackerDbContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            _dbContext = Database.Initializer.TestDatabase();
            Database.Seeder.WithRoles(_dbContext);

            _repository = new RoleRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _repository = null;
        }

        [Test]
        public async Task Create_AddsNewRoleToDatabaseAsync()
        {
            // Arrange
            var newRole = new CustomRole { Name = "Contributor" };

            // Act
            var createdRole = await _repository.Create(newRole);
            var roleInDb = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == newRole.Name);

            // Assert
            Assert.That(createdRole, Is.Not.EqualTo(newRole));
            Assert.Multiple(() =>
            {
                Assert.That(createdRole.Name, Is.EqualTo(newRole.Name));
                Assert.That(roleInDb, Is.Not.Null);
            });
        }

        [Test]
        public async Task GetAllRoles_ReturnsAllRolesInDatabaseAsync()
        {
            // Act
            var result = await _repository.GetAllRoles();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task GetRoleById_RoleExists_ReturnsMatchingRoleAsync()
        {
            // Arrange
            const string roleName = "NonDeletable";
            string roleId = (await _repository.GetRoleByName(roleName)).Id;

            // Act
            var result = await _repository.GetRoleById(roleId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(roleName));
        }


        [Test]
        public async Task GetRoleByName_RoleExists_ReturnsMatchingRoleAsync()
        {
            // Arrange
            const string expectedName = "NonDeletable";

            // Act
            var result = await _repository.GetRoleByName(expectedName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }

        [Test]
        public async Task Update_RoleExists_UpdatesRoleInDatabaseAsync()
        {
            // Arrange

            const string roleName = "RoleA";
            string roleId = (await _repository.GetRoleByName(roleName)).Id;

            var updatedRole = new CustomRole { Id = roleId, Name = "NewName" };

            // Act
            var result = await _repository.Update(updatedRole);

            // Assert
            Assert.That(result.Name, Is.EqualTo("NewName"));
        }


        [Test]
        public async Task Delete_RoleExists_RemovesRoleAndReturnsTrueAsync()
        {
            // Arrange
            const string roleName = "RoleA";
            string roleId = (await _repository.GetRoleByName(roleName)).Id;

            // Act
            var isDeleted = await _repository.Delete(roleId);

            // Assert
            Assert.That(isDeleted, Is.True);
            var roleInDb = await _dbContext.Roles.FindAsync(roleId);
            Assert.That(roleInDb, Is.Null);
        }

        [Test]
        public async Task Delete_RoleExists_NotDeletable_ReturnsFalseAsync()
        {
            // Arrange
            const string roleName = "NonDeletable";
            string roleId = (await _repository.GetRoleByName(roleName)).Id;

            // Act
            var isDeleted = await _repository.Delete(roleId);

            // Assert
            Assert.That(isDeleted, Is.False);
            var roleInDb = await _dbContext.Roles.FindAsync(roleId);
            Assert.That(roleInDb, Is.Not.Null);
        }
    }
}

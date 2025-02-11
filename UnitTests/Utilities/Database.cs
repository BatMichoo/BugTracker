using Core.Entities.BugEntity;
using Core.Entities.UserEntity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Utilities
{
    public static class Database
    {
        public static class Initializer
        {
            public static TrackerDbContext TestDatabase()
            {
                var options = new DbContextOptionsBuilder<TrackerDbContext>()
                                .UseInMemoryDatabase("TestDb")
                                .Options;

                return new TrackerDbContext(options);
            }

            public static TrackerDbContext TestDatabaseWithBugSeeding()
            {
                var context = TestDatabase();

                Seeder.WithBugs(context);

                return context;
            }
        }

        public static class Seeder
        {
            public static void WithBugs(TrackerDbContext context)
            {
                var user = new BugUser { Id = "abc", UserName = "tester1", Name = "Pesho" };
                var user2 = new BugUser { Id = "a", UserName = "tester2", Name = "Gosho" };
                var user3 = new BugUser { Id = "ab", UserName = "tester3", Name = "Sasho" };

                var entity = new Bug { Id = 1, Title = "Title1", AssigneeId = "abc", CreatorId = "a", CreatedOn = DateTime.Parse("1.10.2024"), Description = "test 1234", Priority = 4, Status = 0, LastUpdatedById = "a" };
                var entity2 = new Bug { Id = 2, Title = "Title2", AssigneeId = "abcd", CreatorId = "ab", CreatedOn = DateTime.Parse("3.10.2024"), Description = "test 12345", Priority = 3, Status = 1, LastUpdatedById = "ab" };
                var entity3 = new Bug { Id = 3, Title = "Title3", AssigneeId = "abcd", CreatorId = "abc", CreatedOn = DateTime.Parse("1.11.2024"), Description = "test 123457", Priority = 3, Status = 1, LastUpdatedById = "ab" };

                context.Bugs.AddRange(new List<Bug> { entity, entity2, entity3 });
                context.Users.AddRange(new List<BugUser> { user, user2, user3 });

                context.SaveChanges();
            }
        }        
    }
}

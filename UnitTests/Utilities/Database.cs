using Core.Entities.BugEntity;
using Core.Entities.UserEntity;
using DotNetEnv;
using DotNetEnv.Extensions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Utilities
{
    public static class Database
    {
        public static class Initializer
        {
            private static string GetTestDbConnString()
            {
                var envVars = Env.Load("../../../../.env.development").ToDotEnvDictionary();

                string server = envVars["SQL_SERVER_HOST"];
                string database = envVars["SQL_DATABASE"];
                string userId = envVars["SQL_USER"];
                string password = envVars["SA_PASSWORD"];

                string cleanDbString = "Server={0};Database={1}TestDb;User ID={2};Password={3};MultipleActiveResultSets=true";

                return string.Format(cleanDbString, server, database, userId, password);
            }

            public static TrackerDbContext TestDatabase()
            {
                string dbConnString = GetTestDbConnString();

                var options = new DbContextOptionsBuilder<TrackerDbContext>()
                                .UseSqlServer(dbConnString)
                                .Options;

                var context = new TrackerDbContext(options);

                context.Database.Migrate();

                return context;
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
            public static void WithUsers(TrackerDbContext context)
            {
                var user = new BugUser { UserName = "tester1", Name = "Pesho" };
                var user2 = new BugUser { UserName = "tester2", Name = "Gosho" };
                var user3 = new BugUser { UserName = "tester3", Name = "Sasho" };

                context.Users.AddRange(new List<BugUser> { user, user2, user3 });

                context.SaveChanges();
            }

            public static void WithBugs(TrackerDbContext context)
            {
                WithUsers(context);

                var twoUsers = context.Users.Take(2).ToList();

                var entity = new Bug { Title = "Title1", Description = "test 1234", Priority = 4, Status = 0, CreatedOn = DateTime.Now.AddMonths(-1) };
                var entity2 = new Bug { Title = "Title2", Description = "test 12345", Priority = 3, Status = 1, CreatedOn = DateTime.Now.AddMonths(1) };
                var entity3 = new Bug { Title = "Title3", Description = "test 123457", Priority = 3, Status = 1, CreatedOn = DateTime.Now.AddMonths(5) };

                var bugList = new List<Bug> { entity, entity2, entity3 };

                for (int i = 0; i < bugList.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        bugList[i].AssigneeId = twoUsers[0].Id;
                        bugList[i].CreatorId = twoUsers[0].Id;
                        bugList[i].LastUpdatedById = twoUsers[0].Id;
                    }
                    else
                    {
                        bugList[i].AssigneeId = twoUsers[1].Id;
                        bugList[i].CreatorId = twoUsers[1].Id;
                        bugList[i].LastUpdatedById = twoUsers[1].Id;
                    }
                }

                context.Bugs.AddRange(bugList);

                context.SaveChanges();
            }
        }        
    }
}

using API.AutoMapper;
using AutoMapper;
using Core.AutoMapper;
using Core.BugService;
using Core.DbService;
using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTests
{
    public class BugServiceTests
    {
        private IMapper mapper;
        private Mock<ITrackerDbService>? mockedDbService;

        private IBugService? bugService;
        private BugModel exampleBug;

        [SetUp]
        public void Setup()
        {
            exampleBug =  new BugModel()
            {
                Id = 1,
                AssigneeId = "a",
                CreatorId = "c",
                Description = "test description",
                Priority = BugPriority.Normal,
                Status = BugStatus.InProgress,
                Creator = "Test User",
                Assignee = "Assigned User",
                LastUpdatedBy = "Updater",
                CreatedOn = DateTime.Now,
                LastUpdatedById = "l",
                LastUpdatedOn = DateTime.Now,
                Comments = new List<CommentModel>()
            };

            var services = new ServiceCollection();

            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(typeof(BugProfile));
                opt.AddProfile(typeof(BugUserProfile));
                opt.AddProfile(typeof(CommentProfile));
            });

            var serviceProvider = services.BuildServiceProvider();
            mapper = serviceProvider.GetRequiredService<IMapper>();

            mockedDbService = new Mock<ITrackerDbService>();
        }

        [TearDown]
        public void TearDown()
        {
            mockedDbService = null;
            bugService = null;
        }

        [Test]
        public async Task TestAddBug()
        {
            mockedDbService.Setup(s => s.AddBug(It.Is<BugModel>(b =>
                b.CreatorId == exampleBug.CreatorId &&
                b.Description == exampleBug.Description &&
                b.Priority == exampleBug.Priority &&
                b.Status == exampleBug.Status)))
                .ReturnsAsync(exampleBug);

            bugService = new BugService(mapper, mockedDbService.Object);

            var bugToAdd = new AddBugModel()
            {
                CreatorId = exampleBug.CreatorId,
                Description = exampleBug.Description,
                Priority = exampleBug.Priority,
                Status = exampleBug.Status
            };



            var result = await bugService.AddBug(bugToAdd);



            Assert.True(result.Id == exampleBug.Id &&
                result.Description == exampleBug.Description &&
                result.Priority == exampleBug.Priority &&
                result.Status == exampleBug.Status &&
                result.CreatedBy == exampleBug.Creator &&
                result.AssignedTo == exampleBug.Assignee &&
                result.CreatedOn == exampleBug.CreatedOn &&
                result.LastUpdatedBy == exampleBug.LastUpdatedBy &&
                result.LastUpdatedOn == exampleBug.LastUpdatedOn);
        }

        [Test]
        public async Task TestRetrieveBugById()
        {            
            mockedDbService.Setup<Task<BugModel>>(s => s.GetBugById(exampleBug.Id)).ReturnsAsync(exampleBug);
            bugService = new BugService(mapper, mockedDbService.Object);


            var result = await bugService.RetrieveBugById(exampleBug.Id);


            Assert.That(result.Id == exampleBug.Id &&
                result.Description == exampleBug.Description &&
                result.Priority == exampleBug.Priority &&
                result.Status == exampleBug.Status &&
                result.CreatedBy == exampleBug.Creator &&
                result.AssignedTo == exampleBug.Assignee &&
                result.CreatedOn == exampleBug.CreatedOn &&
                result.LastUpdatedBy == exampleBug.LastUpdatedBy &&
                result.LastUpdatedOn == exampleBug.LastUpdatedOn, Is.True);
        }

        [Test]
        public async Task TestUpdateBug()
        {
            mockedDbService.Setup(s => s.UpdateBug(It.Is<BugModel>(b =>
                b.Id == exampleBug.Id &&
                b.Description == exampleBug.Description &&
                b.Priority == exampleBug.Priority &&
                b.Status == exampleBug.Status)))
                .ReturnsAsync(exampleBug);

            var bugService = new BugService(mapper, mockedDbService.Object);

            var result = await bugService.UpdateBug(mapper.Map<EditBugViewModel>(exampleBug));

            Assert.That(result.Id == exampleBug.Id &&
                result.Description == exampleBug.Description &&
                result.Priority == exampleBug.Priority &&
                result.Status == exampleBug.Status &&
                result.CreatedBy == exampleBug.Creator &&
                result.AssignedTo == exampleBug.Assignee &&
                result.CreatedOn == exampleBug.CreatedOn &&
                result.LastUpdatedBy == exampleBug.LastUpdatedBy &&
                result.LastUpdatedOn == exampleBug.LastUpdatedOn, Is.True);
        }

        [Test]
        public async Task TestDeleteBug()
        {
            mockedDbService.Setup(s => s.DeleteBug(1))
                .ReturnsAsync(true);

            var bugService = new BugService(mapper, mockedDbService.Object);

            bool result = await bugService.DeleteBug(1);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TestReassignBug()
        {
            string newUserId = "2";
            string newUserName = "Gosho";

            var newBugVersion = new BugModel()
            {
                Id = exampleBug.Id,
                Assignee = newUserName,
                AssigneeId = newUserId,
            };

            newBugVersion.AssigneeId = newUserId;
            newBugVersion.Assignee = newUserName;

            mockedDbService.Setup(s => s.GetBugById(exampleBug.Id)).ReturnsAsync(exampleBug);

            mockedDbService.Setup(s => s.UpdateBug(It.Is<BugModel>(b => b.Id == exampleBug.Id && b.AssigneeId == exampleBug.AssigneeId)))
                .ReturnsAsync(newBugVersion);

            var bugService = new BugService(mapper, mockedDbService.Object);

            var result = await bugService.ReassignBug(exampleBug.Id, newUserId);

            Assert.That(result.AssignedTo == newBugVersion.Assignee, Is.True);
        }

        [Test]
        public async Task TestBugClosing()
        {
            var toBeClosed = new BugModel()
            {
                Status = BugStatus.InProgress,
                Priority = BugPriority.High
            };

            var closedBug = new BugModel()
            {
                Id = exampleBug.Id,
                Status = BugStatus.Fixed,
                Priority = BugPriority.None
            };

            mockedDbService.Setup(s => s.GetBugById(exampleBug.Id)).ReturnsAsync(toBeClosed);
            mockedDbService.Setup(s => s.UpdateBug(toBeClosed))
                .ReturnsAsync(closedBug);

            var bugService = new BugService(mapper, mockedDbService.Object);

            var result = await bugService.CloseBugAfterFixing(exampleBug.Id);

            Assert.That(result.Id, Is.EqualTo(exampleBug.Id));
            Assert.That(result.Status, Is.EqualTo(BugStatus.Fixed));
            Assert.That(result.Priority, Is.EqualTo(BugPriority.None));
        }
    }
}
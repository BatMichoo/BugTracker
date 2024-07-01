using AutoMapper;
using Core.DTOs.Bug;
using Infrastructure;
using Infrastructure.Models.Bug;
using Microsoft.EntityFrameworkCore;

namespace Core.DbService
{
    public class TrackerDbService : ITrackerDbService
    {
        private readonly TrackerDbContext dbContext;
        private readonly IMapper mapper;

        public TrackerDbService(TrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<BugModel> AddBug(BugModel bugModel)
        {
            var bug = mapper.Map<Bug>(bugModel);

            bug.CreatedOn = bug.LastUpdatedOn;

            await dbContext.Bugs.AddAsync(bug);

            var changesCount = await dbContext.SaveChangesAsync();

            var createdBug = await GetBug(bug.Id);

            return createdBug!;
        }

        public async Task<bool> DeleteBug(int bugId)
        {
            var bug = await dbContext.Bugs.FirstOrDefaultAsync(b => b.Id == bugId);

            if (bug != null)
            {
                dbContext.Bugs.Remove(bug);

                int changesCount = await dbContext.SaveChangesAsync();

                return changesCount > 0;
            }

            return false;
        }

        public async Task<BugModel> UpdateBug(BugModel bugModel)
        {
            var toBeEdited = await dbContext.Bugs.FirstOrDefaultAsync(b => b.Id == bugModel.Id);

            mapper.Map(bugModel, toBeEdited);

            await dbContext.SaveChangesAsync();

            return await GetBug(toBeEdited.Id);
        }

        public async Task<BugModel?> GetBug(int bugId)
        {
            var bug = await dbContext.Bugs
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bugId);

            if (bug != null)
            {
                var model = mapper.Map<BugModel>(bug);

                return model;
            } 

            return null;
        }

        public async Task<List<BugModel>?> GetBugsWithStatus(BugStatus? status)
        {
            var bugQuery = dbContext.Bugs
                .AsNoTracking();

            if (status == null)
            {
                bugQuery.Where(b => b.Status != BugStatus.Fixed);
            }
            else
            {
                bugQuery.Where(b => b.Status == status);
            }

            var bugs = await bugQuery.ToListAsync();

            if (bugs != null)
            {
                return mapper.Map<List<BugModel>>(bugs);
            }

            return null;
        }        
    }
}

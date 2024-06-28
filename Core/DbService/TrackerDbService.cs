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

        public async Task<BugViewModel> AddBug(BugViewModel bugViewModel)
        {
            var bug = mapper.Map<Bug>(bugViewModel);

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

        public async Task<BugViewModel> EditBug(BugViewModel bugViewModel)
        {
            var toBeEdited = await dbContext.Bugs.FirstOrDefaultAsync(b => b.Id == bugViewModel.Id);

            mapper.Map(bugViewModel, toBeEdited);

            await dbContext.SaveChangesAsync();

            return await GetBug(toBeEdited.Id);
        }

        public async Task<BugViewModel?> GetBug(int bugId)
        {
            var bug = await dbContext.Bugs.FirstOrDefaultAsync(b => b.Id == bugId);

            if (bug != null)
            {
                var viewModel = mapper.Map<BugViewModel>(bug);

                return viewModel;
            } 

            return null;
        }
    }
}

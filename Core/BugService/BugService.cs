using AutoMapper;
using Core.DbService;
using Core.DTOs.Bug;
using Infrastructure.Models.Bug;

namespace Core.BugService
{
    public class BugService : IBugService
    {
        private readonly IMapper mapper;
        private readonly ITrackerDbService dbService;

        public BugService(IMapper mapper, ITrackerDbService dbService)
        {
            this.mapper = mapper;
            this.dbService = dbService;
        }

        /// <summary>
        /// Enters a new bug in the system for tracking.
        /// </summary>
        /// <param name="bug"></param>
        /// <returns></returns>
        public async Task<BugViewModel> AddBug(AddBugViewModel bug)
        {
            var newBug = mapper.Map<BugModel>(bug);

            var createdBug = await dbService.AddBug(newBug);

            if (createdBug != null)
            {
                return mapper.Map<BugViewModel>(createdBug);
            }

            return null;
        }

        /// <summary>
        /// Closes the bug after it's been fixed.
        /// </summary>
        /// <param name="bugId"></param>
        /// <returns></returns>
        public async Task<BugViewModel> CloseBugAfterFixing(int bugId)
        {
            var bug = await dbService.GetBug(bugId);

            if (bug != null)
            {
                bug.Priority = BugPriority.None;
                bug.Status = BugStatus.Fixed;
            }

            var updatedBug = await dbService.UpdateBug(mapper.Map<BugModel>(bug));

            return mapper.Map<BugViewModel>(updatedBug);
        }

        /// <summary>
        /// Removes the selected bug by it's ID.
        /// </summary>
        /// <param name="bugId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBug(int bugId)
        {
            var success = await dbService.DeleteBug(bugId);

            return success;
        }

        /// <summary>
        /// Retrieves a list of all yet to be fixed bugs.
        /// </summary>
        /// <returns></returns>
        public async Task<List<BugViewModel>> RetrieveAllBugs()
        {
            var bugs = await dbService.GetBugsWithStatus(null);

            if (bugs != null)
            {
                return mapper.Map<List<BugViewModel>>(bugs);
            }

            return null;
        }

        /// <summary>
        /// Reassigns a bug to another user for fixing.
        /// </summary>
        /// <param name="bugId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> ReassignBug(int bugId, int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves the selected bug by it's ID.
        /// </summary>
        /// <param name="bugId"></param>
        /// <returns></returns>
        public async Task<BugViewModel> RetrieveBug(int bugId)
        {
            var bug = await dbService.GetBug(bugId);

            if (bug != null )
            {
                return mapper.Map<BugViewModel>(bug);
            }

            return null;
        }

        /// <summary>
        /// Updates an existing bug or creates it if it does not exist.
        /// </summary>
        /// <param name="updatedBug"></param>
        /// <returns></returns>
        public async Task<BugViewModel> UpdateOrCreateBug(EditBugViewModel updatedBug)
        {
            var bug = await dbService.GetBug(updatedBug.Id);

            BugModel newBug = mapper.Map<BugModel>(updatedBug);            

            if (bug != null)
            {
                newBug.CreatedOn = bug.CreatedOn;
                newBug = await dbService.UpdateBug(newBug);
            }
            else
            {
                newBug = await dbService.AddBug(newBug);
            }

            return mapper.Map<BugViewModel>(newBug);
        }

        /// <summary>
        /// Retrieves all bugs with the selected status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<BugViewModel>> RetrieveBugsByStatus(BugStatus status)
        {
            var bugList = await dbService.GetBugsWithStatus(status);

            return mapper.Map<List<BugViewModel>>(bugList);
        }
    }
}

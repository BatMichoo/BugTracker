using AutoMapper;
using Core.DbService;
using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;

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
        /// <param name="bug">The bug to be created. <see cref="AddBugModel"/></param>
        /// <returns>A view model of the created bug <see cref="BugViewModel"/></returns>
        public async Task<BugViewModel> AddBug(AddBugModel bug)
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
        /// <param name="bugId">Id of the selected bug.</param>
        /// <returns>An updated view model of the fixed bug. <see cref="BugViewModel"/></returns>
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
        /// <returns>A boolen value based on the success of the operation.</returns>
        public async Task<bool> DeleteBug(int bugId)
        {
            var success = await dbService.DeleteBug(bugId);

            return success;
        }

        /// <summary>
        /// Retrieves a list of all yet to be fixed bugs.
        /// </summary>
        /// <returns>A list of bug view models. <see cref="BugViewModel"/></returns>
        public async Task<List<BugViewModel>> RetrieveAllActiveBugs()
        {
            var bugs = await dbService.GetActiveBugs();

            if (bugs != null)
            {
                return mapper.Map<List<BugViewModel>>(bugs);
            }

            return new List<BugViewModel>();
        }

        /// <summary>
        /// Reassigns a bug to another user for fixing.
        /// </summary>
        /// <param name="bugId">Bug to be reassigned.</param>
        /// <param name="userId">An user ID to reassign the bug to</param>
        /// <returns>A view model of the reassigned bug.</returns>
        public async Task<BugViewModel> ReassignBug(int bugId, string? userId)
        {
            var bug = await dbService.GetBug(bugId);

            if (bug != null)
            {
                bug.AssigneeId = userId;

                bug = await dbService.UpdateBug(bug);
            }

            return mapper.Map<BugViewModel>(bug);
        }

        /// <summary>
        /// Retrieves the selected bug by it's ID.
        /// </summary>
        /// <param name="bugId"></param>
        /// <returns>A view model of the bug</returns>
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

            return mapper.Map<List<BugViewModel>>(bugList) ?? new List<BugViewModel>();
        }

        public async Task<CommentViewModel> AddComment(AddCommentModel newComment)
        {
            var commentModel = mapper.Map<CommentModel>(newComment);

            var comment = await dbService.AddComment(commentModel);

            return mapper.Map<CommentViewModel>(comment);
        }

        public Task<CommentViewModel> EditComment(EditCommentModel editedComment)
        {
            throw new NotImplementedException();
        }

        public async Task<CommentViewModel> EditLikes(int bugId, char? action)
        {
            int likes;

            switch (action)
            {
                case '-':
                    likes = -1;
                    break;
                default: 
                    likes = 1; 
                    break;
            }

            var updatedBug = await dbService.UpdateLikes(bugId, likes);

            return mapper.Map<CommentViewModel>(updatedBug);
        }
    }
}

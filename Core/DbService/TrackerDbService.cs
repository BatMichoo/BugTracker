using AutoMapper;
using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;
using Infrastructure;
using Infrastructure.Models.Bug;
using Infrastructure.Models.Comment;
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

            await dbContext.SaveChangesAsync();

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
            var toBeEdited = await dbContext.Bugs
                .FirstOrDefaultAsync(b => b.Id == bugModel.Id);
            
            if (toBeEdited != null)
            {
                mapper.Map(bugModel, toBeEdited);

                await dbContext.SaveChangesAsync();
            }

            return (await GetBug(toBeEdited.Id))!;
        }

        public async Task<BugModel?> GetBug(int bugId)
        {
            var bug = await dbContext.Bugs
                .AsNoTracking()
                .Include(b => b.Assignee)
                .Include(b => b.Creator)
                .Include(b => b.LastUpdatedBy)
                .Include(b => b.Comments)
                    .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == bugId);

            if (bug != null)
            {
                var model = mapper.Map<BugModel>(bug);

                return model;
            } 

            return null;
        }

        public async Task<List<BugModel>> GetBugsWithStatus(BugStatus status)
        {
            var bugs = await dbContext.Bugs
                .AsNoTracking()
                .Where(b => b.Status == (int) status)
                .Include(b => b.Creator)
                .Include(b => b.Assignee)
                .Include(b => b.LastUpdatedBy)
                .ToListAsync();

            if (bugs != null)
            {
                return mapper.Map<List<BugModel>>(bugs);
            }

            return new List<BugModel>();
        }

        public async Task<List<BugModel>> GetActiveBugs()
        {
            var bugs = await dbContext.Bugs
                .AsNoTracking()
                .Where(b => b.Status != (int) BugStatus.Fixed)
                .Include(b => b.Creator)
                .Include(b => b.Assignee)
                .Include(b => b.LastUpdatedBy)
                .ToListAsync();

            if (bugs != null)
            {
                return mapper.Map<List<BugModel>>(bugs);
            }

            return new List<BugModel>();
        }

        public async Task<List<CommentModel>> GetComments(int bugId)
        {
            var comments = await dbContext.Comments
                .AsNoTracking()
                .Where(c => c.BugId == bugId)
                .ToListAsync();

            if (comments != null)
            {
                return mapper.Map<List<CommentModel>>(comments);
            }

            return new List<CommentModel>();
        }

        public async Task<CommentModel> AddComment(CommentModel newComment)
        {
            var comment = mapper.Map<Comment>(newComment);

            await dbContext.Comments.AddAsync(comment);

            await dbContext.SaveChangesAsync();

            var savedComment = await dbContext.Comments
                .AsNoTracking()
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            return mapper.Map<CommentModel>(savedComment);
        }

        public async Task<CommentModel> UpdateLikes(int commentId, int likes)
        {
            var comment = await dbContext.Comments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment != null)
            {
                comment.Likes += likes;

                await dbContext.SaveChangesAsync();
            }

            return mapper.Map<CommentModel>(comment);
        }
    }
}

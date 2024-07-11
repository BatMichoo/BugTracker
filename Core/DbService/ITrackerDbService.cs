using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;

namespace Core.DbService
{
    public interface ITrackerDbService
    {
        Task<BugModel> AddBug(BugModel bug);
        Task<BugModel?> GetBug(int bugId);
        Task<BugModel> UpdateBug(BugModel bug);        
        Task<bool> DeleteBug(int bugId);
        Task<List<BugModel>> GetBugsWithStatus(BugStatus status);
        Task<List<BugModel>> GetActiveBugs();

        Task<List<CommentModel>> GetComments(int bugId);
        Task<CommentModel> AddComment(CommentModel newComment);
        Task<CommentModel> UpdateLikes(int bugId, int likes);

    }
}

using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;

namespace Core.BugService
{
    public interface IBugService
    {
        Task<BugViewModel> RetrieveBug(int bugId);
        Task<List<BugViewModel>> RetrieveAllActiveBugs();
        Task<List<BugViewModel>> RetrieveBugsByStatus(BugStatus status);
        Task<BugViewModel> AddBug(AddBugModel newBug);
        Task<bool> DeleteBug(int bugId);
        Task<BugViewModel> UpdateOrCreateBug(EditBugViewModel bug);
        Task<BugViewModel> CloseBugAfterFixing(int bugId);
        Task<BugViewModel> ReassignBug(int bugId, string? userId);
        Task<CommentViewModel> AddComment(AddCommentModel newComment);
        Task<CommentViewModel> EditComment(EditCommentModel editedComment);
        Task<CommentViewModel> EditLikes(int commentId, char? action);
    }
}

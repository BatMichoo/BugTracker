using Core.DTOs;
using Core.DTOs.Comments;

namespace Core.CommentService
{
    public interface ICommentService
    {
        Task<CommentViewModel?> GetById(int id);
        Task<PagedList<CommentViewModel>> GetCommentsByBugId(int bugId);
        Task<CommentViewModel> Create(AddCommentModel comment);
        Task Delete(int id);
    }
}

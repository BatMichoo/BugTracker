using Core.BaseService;
using Core.DTOs;
using Core.DTOs.Comments;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public interface ICommentService : IAdvancedService<Comment, CommentOrderBy, CommentFilterType, CommentModel, AddCommentModel, EditCommentModel>
    {
        Task<PagedList<CommentViewModel>> GetCommentsByBugId(int bugId);
        Task<int> Interact(int commentId, char operation);
    }
}

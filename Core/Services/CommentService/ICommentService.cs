using Core.DTOs.Comments;
using Core.Entities.CommentEntity;
using Core.Services.EntityService;

namespace Core.Services.CommentService
{
    public interface ICommentService : IEntityService<Comment, CommentModel, AddCommentModel, EditCommentModel>
    {
        Task<int> Interact(int commentId, char operation);
        Task<List<CommentModel>> GetByBugId(int bugId, bool isFullyIncluded);
    }
}

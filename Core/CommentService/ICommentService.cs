using Core.BaseService;
using Core.DTOs.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public interface ICommentService : IEntityService<Comment, CommentModel, AddCommentModel, EditCommentModel>
    {
        Task<int> Interact(int commentId, char operation);
    }
}

using Core.DTOs.Comments;
using Core.EntityService;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public interface ICommentService : IEntityService<Comment, CommentModel, AddCommentModel, EditCommentModel>
    {
        Task<int> Interact(int commentId, char operation);
    }
}

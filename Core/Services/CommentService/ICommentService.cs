using Core.DTOs.Comments;
using Core.Services.EntityService;
using Infrastructure.Models.CommentEntity;

namespace Core.Services.CommentService
{
    public interface ICommentService : IEntityService<Comment, CommentModel, AddCommentModel, EditCommentModel>
    {
        Task<int> Interact(int commentId, char operation);
    }
}

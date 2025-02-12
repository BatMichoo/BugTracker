using Core.DTOs.Replies;
using Core.Entities.ReplyEntity;
using Core.Services.EntityService;

namespace Core.Services.ReplyService
{
    public interface IReplyService : IEntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel>
    {
        Task<List<Reply>> GetByCommentId(int commentId);
    }
}
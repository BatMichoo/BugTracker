using Core.DTOs.Replies;
using Core.EntityService;
using Infrastructure.Models.ReplyEntity;

namespace Core.ReplyService
{
    public interface IReplyService : IEntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel>
    {
    }
}
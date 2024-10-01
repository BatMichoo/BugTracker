using Core.BaseService;
using Core.DTOs.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.ReplyService
{
    public interface IReplyService : IEntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel>
    {
    }
}
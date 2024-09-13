using Core.BaseService;
using Core.DTOs.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.ReplyService
{
    public interface IReplyService : ISimpleService<Reply, ReplyModel, AddReplyModel, EditReplyModel>
    {
    }
}
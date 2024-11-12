using Core.DTOs.Replies;
using Core.Services.EntityService;
using Infrastructure.Models.ReplyEntity;

namespace Core.Services.ReplyService
{
    public interface IReplyService : IEntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel>
    {
    }
}
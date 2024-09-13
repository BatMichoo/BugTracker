using AutoMapper;
using Core.BaseService;
using Core.DTOs.Replies;
using Core.Repository.ReplyRepo;
using Infrastructure.Models.ReplyEntity;

namespace Core.ReplyService
{
    public class ReplyService : SimpleService<Reply, ReplyModel, AddReplyModel, EditReplyModel>, IReplyService
    {
        public ReplyService(IReplyRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}

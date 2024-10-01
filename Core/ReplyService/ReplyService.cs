using AutoMapper;
using Core.BaseService;
using Core.DTOs.Replies;
using Core.QueryParameters;
using Core.Repository.ReplyRepo;
using Core.Utilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.ReplyService
{
    public class ReplyService : EntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel, ReplySortBy, ReplyFilterType>, IReplyService
    {
        public ReplyService(IReplyRepository repository, IMapper mapper, IReplyQueryParametersFactory queryParametersFactory)
            : base(repository, mapper, queryParametersFactory)
        {
        }
    }
}

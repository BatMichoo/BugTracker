using AutoMapper;
using Core.DTOs.Replies;
using Core.EntitiesQueryUtilities.QueryParameters.Replies;
using Core.Repository.ReplyRepo;
using Core.Services.EntityService;
using Core.Utilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.Services.ReplyService
{
    public class ReplyService : EntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel, ReplySortBy, ReplyFilterType>, IReplyService
    {
        public ReplyService(IReplyRepository repository, IMapper mapper, IReplyQueryParametersFactory queryParametersFactory)
            : base(repository, mapper, queryParametersFactory)
        {
        }
    }
}

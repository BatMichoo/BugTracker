using AutoMapper;
using Core.DTOs.Replies;
using Core.Entities.ReplyEntity;
using Core.EntitiesQueryUtilities.Replies;
using Core.Repositories;
using Core.Services.EntityService;

namespace Core.Services.ReplyService
{
    public class ReplyService : EntityService<Reply, ReplyModel, AddReplyModel, EditReplyModel, ReplySortBy, ReplyFilterType>, IReplyService
    {
        public ReplyService(IReplyRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task<List<Reply>> GetByCommentId(int commentId)
        {
            var replies = await ((IReplyRepository) _repository).GetByCommentId(commentId);

            return replies;
        }
    }
}

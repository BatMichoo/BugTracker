using Core.EntitiesQueryUtilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.EntitiesQueryUtilities.QueryParameters.Replies
{
    public interface IReplyQueryParametersFactory : IQueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>
    {
    }
}

using Core.Entities.ReplyEntity;
using Core.EntitiesQueryUtilities.Replies;

namespace Core.EntitiesQueryUtilities.QueryParameters.Replies
{
    public interface IReplyQueryParametersFactory : IQueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>
    {
    }
}

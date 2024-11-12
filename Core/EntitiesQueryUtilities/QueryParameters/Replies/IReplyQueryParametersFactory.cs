using Core.EntitiesQueryUtilities.QueryParameters;
using Core.Utilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.EntitiesQueryUtilities.QueryParameters.Replies
{
    public interface IReplyQueryParametersFactory : IQueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>
    {
    }
}

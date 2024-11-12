using Core.Utilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.QueryParameters.Replies
{
    public interface IReplyQueryParametersFactory : IQueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>
    {
    }
}

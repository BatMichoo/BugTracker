using Core.Utilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.QueryParameters
{
    public interface IReplyQueryParametersFactory : IQueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>
    {
    }
}

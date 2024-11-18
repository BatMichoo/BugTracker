using Core.EntitiesQueryUtilities.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.EntitiesQueryUtilities.QueryParameters.Replies
{
    public class ReplyQueryParametersFactory : QueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>, IReplyQueryParametersFactory
    {
        public ReplyQueryParametersFactory(IReplySortingOptionFactory sortingOptionsFactory, IReplyFilterFactory filterFactory) : base(sortingOptionsFactory, filterFactory)
        {
        }
    }
}

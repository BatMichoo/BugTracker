using Core.Entities.ReplyEntity;
using Core.EntitiesQueryUtilities.Replies;

namespace Core.EntitiesQueryUtilities.QueryParameters.Replies
{
    public class ReplyQueryParametersFactory : QueryParametersFactory<Reply, ReplySortBy, ReplyFilterType>, IReplyQueryParametersFactory
    {
        public ReplyQueryParametersFactory(IReplySortingOptionFactory sortingOptionsFactory, IReplyFilterFactory filterFactory) : base(sortingOptionsFactory, filterFactory)
        {
        }
    }
}

using Core.Entities.ReplyEntity;
using Core.EntitiesQueryUtilities.QueryBuilders;

namespace Infrastructure.QueryBuilders
{
    public class ReplyQueryableBuilder : QueryableBuilder<Reply>, IReplyQueryableBuilder
    {
    }
}

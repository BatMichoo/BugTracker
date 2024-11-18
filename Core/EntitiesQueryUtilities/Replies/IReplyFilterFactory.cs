using Infrastructure.Models.ReplyEntity;

namespace Core.EntitiesQueryUtilities.Replies
{
    public interface IReplyFilterFactory : IFilterFactory<Reply, ReplyFilterType>
    {
    }
}

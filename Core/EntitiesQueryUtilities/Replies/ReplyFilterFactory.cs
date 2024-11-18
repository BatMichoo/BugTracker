using Core.EntitiesQueryUtilities.Replies.Filters;
using Infrastructure.Models.ReplyEntity;

namespace Core.EntitiesQueryUtilities.Replies
{
    public class ReplyFilterFactory : IReplyFilterFactory
    {
        public IFilter<Reply> CreateFilter(ReplyFilterType filterBy, string? value = null)
        {
            switch (filterBy)
            {
                case ReplyFilterType.CommentId:
                    int commentId = value == null ? 0 : int.Parse(value);
                    return new ReplyToCommentFilter(commentId);

                default:
                    throw new ArgumentException("No such filter.");
            }
        }

        public IList<IFilter<Reply>> CreateFilters(string? filter = null)
        {
            return new List<IFilter<Reply>>();
        }
    }
}

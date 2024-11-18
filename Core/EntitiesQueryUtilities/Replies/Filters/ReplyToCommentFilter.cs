using Infrastructure.Models.ReplyEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Replies.Filters
{
    public class ReplyToCommentFilter : IFilter<Reply>
    {
        public ReplyToCommentFilter(int commentId)
        {
            _commentId = commentId;
        }
        private readonly int _commentId;
        public Expression<Func<Reply, bool>> Apply()
            => r => r.CommentId == _commentId;
    }
}

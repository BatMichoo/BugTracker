using Infrastructure.Models.CommentEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Comments.Filters
{
    public class CommentCreatedByFilter : IFilter<Comment>
    {
        private readonly string userId;

        public CommentCreatedByFilter(string userId)
        {
            this.userId = userId;
        }

        public Expression<Func<Comment, bool>> Apply()
            => c => c.AuthorId == userId;
    }
}

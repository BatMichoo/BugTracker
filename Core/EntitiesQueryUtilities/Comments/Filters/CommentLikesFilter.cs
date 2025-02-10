using Core.Entities.CommentEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Comments.Filters
{
    public class CommentLikesFilter : IFilter<Comment>
    {
        private readonly int count;
        private readonly string operation;

        public CommentLikesFilter(int count, string operation)
        {
            this.count = count;
            this.operation = operation;
        }

        public Expression<Func<Comment, bool>> Apply()
        {
            switch (operation)
            {
                case "=>":
                    return c => c.Likes >= count;
                case ">":
                    return c => c.Likes > count;
                case "<=":
                    return c => c.Likes <= count;
                case "<":
                    return c => c.Likes < count;
                default:
                    return c => c.Likes == count;
            }
        }
    }
}

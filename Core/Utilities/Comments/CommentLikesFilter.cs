using Infrastructure.Models.CommentEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Comments
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

        public Expression<Func<Comment, bool>> ToExpression()
        {
            switch (operation)
            {
                case "=>":
                    return c => c.Likes >= count;
                default:
                    return c => c.Likes <= count;
            }
        }
    }
}

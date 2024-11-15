using Infrastructure.Models.CommentEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Comments
{
    public class CommentPostedOnFilter : IFilter<Comment>
    {
        private readonly DateTime targetDate;
        private readonly string operation;

        public CommentPostedOnFilter(DateTime targetDate, string operation)
        {
            this.targetDate = targetDate;
            this.operation = operation;
        }

        public Expression<Func<Comment, bool>> Apply()
        {
            switch (operation)
            {
                case "=>":
                    return c => c.PostedOn >= targetDate;
                default:
                    return c => c.PostedOn <= targetDate;
            }
        }
    }
}

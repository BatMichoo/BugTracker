using Infrastructure.Models.CommentEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Comments
{
    public class CommentByBugIdFilter : IFilter<Comment>
    {
        private readonly int bugId;

        public CommentByBugIdFilter(int bugId)
        {
            this.bugId = bugId;
        }

        public Expression<Func<Comment, bool>> ToExpression()
            => c => c.BugId == bugId;
    }
}

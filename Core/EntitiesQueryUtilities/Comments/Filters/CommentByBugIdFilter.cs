using Core.Entities.CommentEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Comments.Filters
{
    public class CommentByBugIdFilter : IFilter<Comment>
    {
        private readonly int bugId;

        public CommentByBugIdFilter(int bugId)
        {
            this.bugId = bugId;
        }

        public Expression<Func<Comment, bool>> Apply()
            => c => c.BugId == bugId;

        public override bool Equals(object? obj)
        {
            if (obj.GetType() != typeof(CommentByBugIdFilter))
            {
                return false;
            }

            var otherFilter = (CommentByBugIdFilter)obj;

            if (otherFilter.bugId != bugId)
            {
                return false;
            }

            return true;
        }
    }
}

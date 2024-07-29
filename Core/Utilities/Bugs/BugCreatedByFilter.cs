using Infrastructure.Models.BugEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public class BugCreatedByFilter : IFilter<Bug>
    {
        private readonly string userId;

        public BugCreatedByFilter(string userId)
        {
            this.userId = userId;
        }

        public Expression<Func<Bug, bool>> ToExpression()
            => b => b.CreatorId == userId;
    }
}
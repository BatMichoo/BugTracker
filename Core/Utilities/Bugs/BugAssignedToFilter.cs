using Infrastructure.Models.BugEntity;
using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public class BugAssignedToFilter : IFilter<Bug>
    {
        private readonly string userId;

        public BugAssignedToFilter(string userId)
        {
            this.userId = userId;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.AssigneeId == userId;
    }
}
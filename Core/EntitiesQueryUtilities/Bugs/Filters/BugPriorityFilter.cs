using Core.Entities.BugEntity;
using Core.Models.Bugs.BugEnums;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugPriorityFilter : IFilter<Bug>
    {
        private readonly BugPriority _priority;

        public BugPriorityFilter(BugPriority priority)
        {
            _priority = priority;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.Priority == (int) _priority;
    }
}

using Core.Entities.BugEntity;
using Core.Models.Bugs.BugEnums;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugPriorityFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugPriorityFilter);
        private readonly BugPriority _priority;

        public BugPriorityFilter(BugPriority priority) : base(_name, priority.ToString())
        {
            _priority = priority;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.Priority == (int) _priority;
    }
}

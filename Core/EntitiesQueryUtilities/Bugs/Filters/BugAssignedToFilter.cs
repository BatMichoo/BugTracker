using Core.Entities.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugAssignedToFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugAssignedToFilter);
        private readonly string? _userId;

        public BugAssignedToFilter(string? userId) : base(_name, userId)
        {
            _userId = userId;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.AssigneeId == _userId;
    }
}
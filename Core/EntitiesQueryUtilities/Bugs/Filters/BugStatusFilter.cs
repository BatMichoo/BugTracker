using Core.Entities.BugEntity;
using Core.Models.Bugs.BugEnums;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugStatusFilter : IFilter<Bug>
    {
        private readonly BugStatus _status;

        public BugStatusFilter(BugStatus status)
        {
            _status = status;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.Status == (int)_status;
    }
}

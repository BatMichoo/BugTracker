using Core.Entities.BugEntity;
using Core.Models.Bugs.BugEnums;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugStatusFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugFilterType.Status);
        private readonly BugStatus _status;

        public BugStatusFilter(BugStatus status) : base(_name, ((int)status).ToString())
        {
            _status = status;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.Status == (int)_status;
    }
}

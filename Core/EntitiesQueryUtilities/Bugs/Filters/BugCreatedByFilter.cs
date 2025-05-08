using Core.Entities.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugCreatedByFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugFilterType.CreatedBy);
        private readonly string _userId;

        public BugCreatedByFilter(string userId) : base(_name, userId)
        {
            _userId = userId;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.CreatorId == _userId;
    }
}
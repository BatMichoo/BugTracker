using Infrastructure.Models.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugIdFilter : IFilter<Bug>
    {
        private readonly int _id;

        public BugIdFilter(int id)
        {
            _id = id;
        }

        public Expression<Func<Bug, bool>> Apply()
        {
            return b => b.Id == _id;
        }
    }
}

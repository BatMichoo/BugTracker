using Core.Entities.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugIdFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugIdFilter);
        private readonly int _id;

        public BugIdFilter(int id) : base(_name, id.ToString())
        {
            _id = id;
        }

        public Expression<Func<Bug, bool>> Apply()
        {
            return b => b.Id == _id;
        }
    }
}

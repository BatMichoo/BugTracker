using Core.Entities.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugTitleFilter : Filter, IFilter<Bug>
    {
        private const string _name = nameof(BugTitleFilter);
        private readonly string _searchedTitle;

        public BugTitleFilter(string searchedTitle) : base(_name, searchedTitle)
        {
            _searchedTitle = searchedTitle;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.Title.Contains(_searchedTitle);
    }
}

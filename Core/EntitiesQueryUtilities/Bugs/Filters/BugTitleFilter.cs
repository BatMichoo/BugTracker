using Core.Entities.BugEntity;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities.Bugs.Filters
{
    public class BugTitleFilter : IFilter<Bug>
    {
        private readonly string _searchedTitle;

        public BugTitleFilter(string searchedTitle)
        {
            _searchedTitle = searchedTitle;
        }

        public Expression<Func<Bug, bool>> Apply()
            => b => b.Title.Contains(_searchedTitle);
    }
}

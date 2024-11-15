using Infrastructure.Models.BugEntity;

namespace Core.EntitiesQueryUtilities.Bugs
{
    public interface IBugSortingOptionsFactory : ISortingOptionsFactory<Bug, BugSortBy>
    {
    }
}

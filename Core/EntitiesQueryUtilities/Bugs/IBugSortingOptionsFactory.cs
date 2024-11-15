using Infrastructure.Models.BugEntity;

namespace Core.Utilities.Bugs
{
    public interface IBugSortingOptionsFactory : ISortingOptionsFactory<Bug, BugSortBy>
    {
    }
}

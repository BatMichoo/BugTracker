using Infrastructure.Models.BugEntity;

namespace Core.Utilities.Bugs
{
    public interface IBugFilterFactory : IFilterFactory<Bug, BugFilterType>
    {
    }
}

using Infrastructure.Models.BugEntity;

namespace Core.EntitiesQueryUtilities.Bugs
{
    public interface IBugFilterFactory : IFilterFactory<Bug, BugFilterType>
    {
    }
}

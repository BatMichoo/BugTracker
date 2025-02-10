using Core.Entities.BugEntity;

namespace Core.EntitiesQueryUtilities.Bugs
{
    public interface IBugFilterFactory : IFilterFactory<Bug, BugFilterType>
    {
    }
}

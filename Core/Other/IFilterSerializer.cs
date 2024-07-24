using Core.Utilities.Bugs;

namespace Core.Other
{
    public interface IFilterSerializer<T> where T : class
    {
        string SerializeFilter(IFilter<T> filter);
        IFilter<T> DeserializeFilter(string filterQuery);
    }
}

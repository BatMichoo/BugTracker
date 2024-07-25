namespace Core.Utilities
{
    public interface IFilterFactory<T> where T : class
    {
        Task<IList<IFilter<T>>> CreateFilters(string filter);
    }
}

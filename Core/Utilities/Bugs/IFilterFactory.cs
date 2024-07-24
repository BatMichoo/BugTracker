namespace Core.Utilities.Bugs
{
    public interface IFilterFactory<T> where T : class
    {
        Task<IList<IFilter<T>>> CreateFilters(string filter);
    }
}

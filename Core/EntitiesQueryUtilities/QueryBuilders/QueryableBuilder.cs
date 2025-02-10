using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Core.EntitiesQueryUtilities.QueryBuilders
{
    public abstract class QueryableBuilder<T> : IQueryableBuilder<T> where T : BaseModel
    {
        public IQueryable<T> BuildQuery(IQueryable<T> query, QueryParameters<T>? queryParameters)
        {
            query = ApplyQueryParameters(query, queryParameters);

            query = ApplyPagination(query, queryParameters.PagingInfo);

            return query;
        }

        public IQueryable<T> BuildCountQuery(IQueryable<T> query, QueryParameters<T>? queryParameters)
        {
            query = ApplyQueryParameters(query, queryParameters);

            return query;
        }

        private IQueryable<T> ApplyQueryParameters(IQueryable<T> query, QueryParameters<T>? queryParameters)
        {
            if (queryParameters is not null)
            {
                if (queryParameters.Filters != null && queryParameters.Filters.Count > 0)
                {
                    query = ApplyFilter(query, queryParameters.Filters);
                }

                if (queryParameters.SearchTerm != null)
                {
                    query = ApplySearch(query, queryParameters.SearchTerm);
                }

                if (queryParameters.SortOptions != null)
                {
                    query = ApplySort(query, queryParameters.SortOptions);
                }
            }

            return query;
        }

        protected virtual IQueryable<T> ApplySearch(IQueryable<T> query, string searchTerm)
        {
            return query;
        }

        private static IQueryable<T> ApplyFilter(IQueryable<T> query, IList<IFilter<T>> filters)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter.Apply());
            }

            return query;
        }

        private static IQueryable<T> ApplyPagination(IQueryable<T> query, PagingInfo paging)
        {
            int itemsToSkip = (paging.CurrentPage - 1) * paging.ElementsPerPage;

            if (itemsToSkip > 0)
            {
                query = query.Skip(itemsToSkip);
            }

            if (paging.ElementsPerPage > 0)
            {
                query = query.Take(paging.ElementsPerPage);
            }

            return query;
        }

        private static IQueryable<T> ApplySort(IQueryable<T> query, ISortingOptions<T> sortOptions)
        {
            switch (sortOptions.SortOrder)
            {
                case SortOrder.Descending:
                    query = query.OrderByDescending(sortOptions.Sort());
                    break;
                default:
                    query = query.OrderBy(sortOptions.Sort());
                    break;
            }

            return query;
        }

        
    }
}

using Core.Entities;
using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.QueryBuilders;
using Core.EntitiesQueryUtilities.QueryParameters;

namespace Infrastructure.QueryBuilders
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

                if (queryParameters.SortOptions != null && queryParameters.SortOptions.Count > 0)
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

        private static IQueryable<T> ApplySort(IQueryable<T> query, IList<ISortingOptions<T>> sortOptions)
        {
            IOrderedQueryable<T> orderedQuery;

            var firstSort = sortOptions.First();

            switch (firstSort.SortOrder)
            {
                case SortOrder.Descending:
                    orderedQuery = query
                        .OrderByDescending(firstSort.Sort());
                    break;
                default:
                    orderedQuery = query
                        .OrderBy(firstSort.Sort());
                    break;
            }

            if (sortOptions.Count > 1)
            {
                foreach (var sortOption in sortOptions.Skip(1))
                {
                    switch (sortOption.SortOrder)
                    {
                        case SortOrder.Descending:
                            orderedQuery = orderedQuery
                                .ThenByDescending(sortOption.Sort());
                            break;
                        default:
                            orderedQuery = orderedQuery
                                .ThenBy(sortOption.Sort());
                            break;
                    }
                }
            }

            return orderedQuery;
        }
    }
}

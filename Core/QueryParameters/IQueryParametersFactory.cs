using Infrastructure.Models;

namespace Core.QueryParameters
{
    public interface IQueryParametersFactory<TEntity, TSortBy, TFilterBy> 
        where TEntity : BaseEntity
        where TSortBy : struct, Enum
        where TFilterBy : struct, Enum
    {
        public Task<QueryParameters<TEntity>> ProcessQueryParametersInput(int pageInput, int pageSizeInput, string? searchTermInput,
            string? sortOptionsInput, string? filterInput);

        public Task<QueryParameters<TEntity>> CreateGetAllQuery();
    }
}
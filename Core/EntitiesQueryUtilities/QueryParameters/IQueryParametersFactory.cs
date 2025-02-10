using Core.Entities;

namespace Core.EntitiesQueryUtilities.QueryParameters
{
    public interface IQueryParametersFactory<TEntity, TSortBy, TFilterBy>
        where TEntity : BaseModel
        where TSortBy : struct, Enum
        where TFilterBy : struct, Enum
    {
        public Task<QueryParameters<TEntity>> ProcessQueryParametersInput(int pageInput, int pageSizeInput, string? searchTermInput,
            string? sortOptionsInput, string? filterInput);

        public QueryParameters<TEntity> CreateGetAllQuery();
    }
}
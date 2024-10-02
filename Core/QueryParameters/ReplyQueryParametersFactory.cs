using Infrastructure.Models.ReplyEntity;

namespace Core.QueryParameters
{
    public class ReplyQueryParametersFactory : IReplyQueryParametersFactory
    {
        public QueryParameters<Reply> CreateGetAllQuery()
            => new QueryParameters<Reply>();

        public Task<QueryParameters<Reply>> ProcessQueryParametersInput(int pageInput, int pageSizeInput, string? searchTermInput, string? sortOptionsInput, string? filterInput)
        {
            throw new NotImplementedException();
        }
    }
}

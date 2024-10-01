using Infrastructure.Models.ReplyEntity;

namespace Core.QueryParameters
{
    public class ReplyQueryParametersFactory : IReplyQueryParametersFactory
    {
        public Task<QueryParameters<Reply>> CreateGetAllQuery()
            => Task.FromResult(new QueryParameters<Reply>());

        public Task<QueryParameters<Reply>> ProcessQueryParametersInput(int pageInput, int pageSizeInput, string? searchTermInput, string? sortOptionsInput, string? filterInput)
        {
            throw new NotImplementedException();
        }
    }
}

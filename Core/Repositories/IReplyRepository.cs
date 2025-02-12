using Core.Entities.ReplyEntity;

namespace Core.Repositories
{
    public interface IReplyRepository : IRepository<Reply>
    {
        Task<List<Reply>> GetByCommentId(int commentId);
    }
}

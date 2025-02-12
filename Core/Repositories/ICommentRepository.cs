using Core.Entities.CommentEntity;

namespace Core.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<List<Comment>> GetByBugId(int bugId);
    }
}

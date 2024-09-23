using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.QueryParameters
{
    public interface ICommentQueryFactory : IQueryFactory<Comment, CommentOrderBy, CommentFilterType>
    {
        QueryParameters<Comment> GetByBugId(int bugId);
    }
}

using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.QueryParameters
{
    public interface ICommentQueryParametersFactory : IQueryParametersFactory<Comment, CommentOrderBy, CommentFilterType>
    {
        QueryParameters<Comment> GetByBugId(int bugId);
    }
}

using Core.Entities.CommentEntity;
using Core.EntitiesQueryUtilities.Comments;

namespace Core.EntitiesQueryUtilities.QueryParameters.Comments
{
    public interface ICommentQueryParametersFactory : IQueryParametersFactory<Comment, CommentOrderBy, CommentFilterType>
    {
        QueryParameters<Comment> GetByBugId(int bugId);
    }
}

using Core.EntitiesQueryUtilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.EntitiesQueryUtilities.QueryParameters.Comments
{
    public interface ICommentQueryParametersFactory : IQueryParametersFactory<Comment, CommentOrderBy, CommentFilterType>
    {
        QueryParameters<Comment> GetByBugId(int bugId);
    }
}

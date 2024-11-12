using Core.EntitiesQueryUtilities.QueryBuilders;
using Infrastructure.Models.CommentEntity;

namespace Core.EntitiesQueryUtilities.QueryBuilders.Comments
{
    public interface ICommentQueryableBuilder : IQueryableBuilder<Comment>
    {
    }
}

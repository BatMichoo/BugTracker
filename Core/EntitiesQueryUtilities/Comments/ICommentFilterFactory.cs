using Core.Entities.CommentEntity;

namespace Core.EntitiesQueryUtilities.Comments
{
    public interface ICommentFilterFactory : IFilterFactory<Comment, CommentFilterType>
    {
    }
}

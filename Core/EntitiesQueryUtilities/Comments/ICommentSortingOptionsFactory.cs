using Core.Entities.CommentEntity;

namespace Core.EntitiesQueryUtilities.Comments
{
    public interface ICommentSortingOptionsFactory : ISortingOptionsFactory<Comment, CommentOrderBy>
    {
    }
}

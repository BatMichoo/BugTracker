using Infrastructure.Models.CommentEntity;

namespace Core.Utilities.Comments
{
    public interface ICommentSortingOptionsFactory : ISortingOptionsFactory<Comment, CommentOrderBy>
    {
    }
}

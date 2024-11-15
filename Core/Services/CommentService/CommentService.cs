using AutoMapper;
using Core.DTOs.Comments;
using Core.EntitiesQueryUtilities.Comments;
using Core.EntitiesQueryUtilities.QueryParameters.Comments;
using Core.Repository.CommentRepo;
using Core.Services.EntityService;
using Infrastructure.Models.CommentEntity;

namespace Core.Services.CommentService
{
    public class CommentService : EntityService<Comment, CommentModel, AddCommentModel, EditCommentModel, CommentOrderBy, CommentFilterType>, ICommentService
    {
        public CommentService(ICommentRepository repository, ICommentQueryParametersFactory queryableParametersFactory, IMapper mapper)
            : base(repository, mapper, queryableParametersFactory)
        {
        }

        public async Task<int> Interact(int commentId, char operation)
        {
            var comment = await _repository.GetById(commentId);

            if (comment is null)
            {
                return -1;
            }

            if (operation == '-' && comment.Likes > 0)
            {
                comment.Likes--;
            }
            else if (operation != '-')
            {
                comment.Likes++;
            }

            if (comment.Likes >= 0)
            {
                comment = await _repository.Update(comment);
            }

            return comment.Likes;
        }
    }
}

using AutoMapper;
using Core.BaseService;
using Core.DTOs.Comments;
using Core.QueryParameters;
using Core.Repository.CommentRepo;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
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

            if (operation == '-')
            {
                comment.Likes--;
            }
            else
            {
                comment.Likes++;
            }

            comment = await _repository.Update(comment);

            return comment.Likes;
        }        
    }
}

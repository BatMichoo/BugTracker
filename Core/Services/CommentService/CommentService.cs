using AutoMapper;
using Core.DTOs.Comments;
using Core.Entities.CommentEntity;
using Core.EntitiesQueryUtilities.Comments;
using Core.Repositories;
using Core.Services.EntityService;

namespace Core.Services.CommentService
{
    public class CommentService : EntityService<Comment, CommentModel, AddCommentModel, EditCommentModel, CommentOrderBy, CommentFilterType>, ICommentService
    {
        public CommentService(ICommentRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task<List<CommentModel>> GetByBugId(int bugId)
        {
            var comments = await ((ICommentRepository) _repository).GetByBugId(bugId);

            var models = _mapper.Map<List<CommentModel>>(comments);

            return models;
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

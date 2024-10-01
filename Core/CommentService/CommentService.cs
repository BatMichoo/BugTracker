using AutoMapper;
using Core.BaseService;
using Core.DTOs.Comments;
using Core.Repository.CommentRepo;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public class CommentService : AdvancedService<Comment, CommentOrderBy, CommentFilterType, CommentModel, AddCommentModel, EditCommentModel>, ICommentService
    {
        public CommentService(ICommentRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }        

        public async Task<int> Interact(int commentId, char operation)
        {
            var comment = await AdvancedRepository.GetById(commentId);

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

            comment = await AdvancedRepository.Update(comment);

            return comment.Likes;
        }        
    }
}

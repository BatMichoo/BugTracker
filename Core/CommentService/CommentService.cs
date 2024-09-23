using AutoMapper;
using Core.BaseService;
using Core.DTOs;
using Core.DTOs.Comments;
using Core.QueryParameters;
using Core.Repository.CommentRepo;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public class CommentService : AdvancedService<Comment, CommentOrderBy, CommentFilterType, CommentModel, AddCommentModel, EditCommentModel>, ICommentService
    {
        private ICommentQueryFactory QueryFactory => (ICommentQueryFactory) _queryFactory;

        public CommentService(ICommentRepository repository, ICommentQueryFactory queryFactory, IMapper mapper)
            : base(repository, mapper, queryFactory)
        {
        }

        public async Task<PagedList<CommentModel>> GetByBugId(int bugId)
        {            
            var queryParameters = QueryFactory.GetByBugId(bugId);

            var comments = await AdvancedRepository.RunQuery(queryParameters);

            return new PagedList<CommentModel> { Items = _mapper.Map<List<CommentModel>>(comments)};
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

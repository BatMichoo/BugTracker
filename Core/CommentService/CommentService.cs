using AutoMapper;
using Core.BaseService;
using Core.DTOs;
using Core.DTOs.Comments;
using Core.QueryParameters;
using Core.Repository.CommentRepo;
using Core.Utilities;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public class CommentService : AdvancedService<Comment, CommentOrderBy, CommentFilterType, CommentModel, AddCommentModel, EditCommentModel>, ICommentService
    {
        public CommentService(ICommentRepository repository, ICommentFilterFactory filterFactory, ICommentSortingOptionsFactory sortingOptionsFactory, IMapper mapper) : base(repository, mapper, sortingOptionsFactory, filterFactory)
        {
        }

        public async Task<PagedList<CommentViewModel>> GetCommentsByBugId(int bugId)
        {
            var filter = _filterFactory.CreateFilter(CommentFilterType.BugId, bugId.ToString());
            var sortingOptions = _sortingOptionsFactory.CreateSortingOptions(SortOrder.Ascending, CommentOrderBy.Id);

            var filterList = new List<IFilter<Comment>>() { filter };

            var queryParameters = new QueryParameters<Comment>(filterList, sortOptions: sortingOptions);

            var comments = await _advancedRepository.RunQuery(queryParameters);

            return new PagedList<CommentViewModel> { Items = _mapper.Map<List<CommentViewModel>>(comments)};
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

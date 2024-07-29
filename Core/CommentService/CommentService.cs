using AutoMapper;
using Core.DTOs;
using Core.DTOs.Comments;
using Core.Repository.CommentRepo;
using Core.Utilities;
using Core.Utilities.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly ICommentFilterFactory _filterFactory;
        private readonly ICommentSortingOptionsFactory _sortingOptionsFactory;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, ICommentFilterFactory commentFilterFactory, ICommentSortingOptionsFactory commentSortingOptionsFactory, IMapper mapper)
        {
            _repository = commentRepository;
            _filterFactory = commentFilterFactory;
            _sortingOptionsFactory = commentSortingOptionsFactory;
            _mapper = mapper;
        }

        public async Task<CommentViewModel> Create(AddCommentModel comment)
        {
            var newComment = await _repository.Create(_mapper.Map<Comment>(comment));

            return _mapper.Map<CommentViewModel>(newComment);
        }

        public async Task Delete(int id)
        {
            await _repository.DeleteById(id);
        }

        public async Task<CommentViewModel?> GetById(int id)
        {
            var comment = await _repository.GetById(id);

            if (comment != null)
            {
                return _mapper.Map<CommentViewModel>(comment);
            }

            return null;
        }

        public async Task<PagedList<CommentViewModel>> GetCommentsByBugId(int bugId)
        {
            var filter = await _filterFactory.CreateFilter(CommentFilterType.BugId, bugId);
            var sortingOptions = _sortingOptionsFactory.CreateSortingOptions(SortOrder.Ascending, CommentOrderBy.Id);

            var filterList = filter == null ? new List<IFilter<Comment>>() : new List<IFilter<Comment>>() { filter };

            var comments = await _repository.RetrieveData(filterList, sortingOptions, null, null);

            return new PagedList<CommentViewModel> { Items = _mapper.Map<List<CommentViewModel>>(comments)};
        }
    }
}

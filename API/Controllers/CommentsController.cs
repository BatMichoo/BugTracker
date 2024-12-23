using API.Utilities.ErrorMessages;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.Comments;
using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.Comments.Filters;
using Core.EntitiesQueryUtilities.QueryParameters.Comments;
using Core.Other;
using Core.Services.CommentService;
using Core.Services.UserService;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("bugs/{bugId}/comments")]
    [Authorize(Policy = AuthorizePolicy.UserAccess)]
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly ICommentQueryParametersFactory _queryFactory;
        private readonly IUserService<BugUser> _userService;
        private readonly IMapper _mapper;

        public CommentsController(ICommentService commentService, IUserService<BugUser> userService,
            IMapper mapper, ICommentQueryParametersFactory commentQueryFactory)
        {
            _commentService = commentService;
            _userService = userService;
            _mapper = mapper;
            _queryFactory = commentQueryFactory;
        }

        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetComment(int bugId, int commentId)
        {
            var comment = await _commentService.GetById(commentId);

            if (comment is not null)
            {
                if (comment.BugId != bugId)
                {
                    return BadRequest(new
                    {
                        errorMessage = ErrorMessage.Comments.InvalidBugIdCommentIdPairing,
                        bugId,
                        commentId
                    });
                }

                return Ok(_mapper.Map<CommentViewModel>(comment));
            }

            return NotFound(new
            {
                errorMessage = string.Format(ErrorMessage.Comments.NotFound, commentId),
                commentId
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentsByBugId(int bugId, string? searchTerm, string? sortOptions, string? filter,
            int pageInput = PagingDefaults.StartingPageNumber, int pageSizeInput = PagingDefaults.ElementsPerPage)
        {
            var queryParameters = await _queryFactory.ProcessQueryParametersInput(pageInput, pageSizeInput, searchTerm, sortOptions, filter);

            var inputFilter = queryParameters.Filters.FirstOrDefault(f => f.GetType() == typeof(CommentByBugIdFilter));

            var bugIdFilter = _queryFactory.GetByBugId(bugId).Filters[0];

            if (inputFilter is not null)
            {
                int index = queryParameters.Filters.IndexOf(inputFilter);

                if (!inputFilter.Equals(bugIdFilter))
                {
                    queryParameters.Filters[index] = bugIdFilter;
                }
            }
            else
            {
                queryParameters.Filters.Add(bugIdFilter);
            }

            var comments = await _commentService.Fetch(queryParameters);

            return Ok(_mapper.Map<PagedList<CommentViewModel>>(comments));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostComment(int bugId, AddCommentViewModel comment)
        {
            string userId = _userService.RetrieveUserId();

            var newComment = new AddCommentModel()
            {
                AuthorId = userId,
                BugId = bugId,
                Content = comment.Content
            };

            var createdComment = await _commentService.Create(newComment);

            if (createdComment is not null)
            {
                string uri = Url.Action(nameof(GetComment), "Comments", new { bugId = createdComment.BugId, commentId = createdComment.Id })!;

                return Created(uri, _mapper.Map<CommentViewModel>(createdComment));
            }

            return BadRequest(new
            {
                errorMessage = ErrorMessage.Comments.UnableToCreate
            });
        }

        [HttpDelete("{commentId}")]
        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommentById(int commentId)
        {
            var comment = await _commentService.GetById(commentId);

            if (comment is null) 
            {
                return BadRequest();
            }

            await _commentService.Delete(commentId);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateComment(int bugId, EditCommentViewModel editModel)
        {
            bool doesExist = await _commentService.DoesExist(editModel.Id);

            if (!doesExist)
            {
                return await PostComment(bugId, _mapper.Map<AddCommentViewModel>(editModel));
            }

            var updatedModel = _mapper.Map<EditCommentModel>(editModel);

            var updatedComment = await _commentService.Update(updatedModel);

            return Ok(_mapper.Map<CommentViewModel>(updatedComment));
        }

        [HttpGet("{commentId}/react")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InteractWithComment(int commentId, char operation)
        {
            int currentLikes = await _commentService.Interact(commentId, operation);

            if (currentLikes < 0)
            {
                return NotFound(new
                {
                    errorMessage = string.Format(ErrorMessage.Comments.NotFound, commentId),
                    commentId
                });
            }

            return Ok(currentLikes);
        }
    }
}

using API.Utilities.ErrorMessages;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.Comments;
using Core.EntitiesQueryUtilities.QueryParameters.Comments;
using Core.Other;
using Core.Services.CommentService;
using Core.UserService;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("{bugId}/comments")]
    public class CommentsController : Controller
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
        public async Task<IActionResult> GetCommentsByBugId(int bugId)
        {
            var queryParameters = _queryFactory.GetByBugId(bugId);

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
        [Authorize(Policy = AuthorizePolicy.ElevatedAccess)]
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
            var comment = await _commentService.GetById(editModel.Id);

            if (comment is null)
            {
                return await PostComment(bugId, _mapper.Map<AddCommentViewModel>(editModel));
            }

            var updatedComment = await _commentService.Update(_mapper.Map<EditCommentModel>(editModel));

            return Ok(_mapper.Map<CommentViewModel>(updatedComment));
        }

        [HttpGet("{commentId}/react")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InteractWithComment(int commentId, char operation)
        {
            var comment = await _commentService.GetById(commentId);

            if (comment is null)
            {
                return NotFound(new
                {
                    errorMessage = string.Format(ErrorMessage.Comments.NotFound, commentId),
                    commentId
                });
            }

            int currentLikes = await _commentService.Interact(comment.Id, operation);

            return Ok(currentLikes);
        }
    }
}

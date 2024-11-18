using API.Utilities.ErrorMessages;
using AutoMapper;
using Core.DTOs.Replies;
using Core.Other;
using Core.Services.ReplyService;
using Core.Services.UserService;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("{bugId}/comments/{commentId}/replies")]
    [Authorize(Policy = AuthorizePolicy.BasicAccess)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class RepliesController : BaseController
    {
        private readonly IReplyService _replyService;
        private readonly IUserService<BugUser> _userService;
        private readonly IMapper _mapper;

        public RepliesController(IReplyService replyService, IUserService<BugUser> userService, IMapper mapper)
        {
            _replyService = replyService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{replyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReplyViewModel>> Get(int replyId)
        {
            var reply = await _replyService.GetById(replyId);

            if (reply is not null)
            {
                return Ok(_mapper.Map<ReplyViewModel>(reply));
            }

            return NotFound(new
            {
                errorMessage = string.Format(ErrorMessage.Replies.NotFound, replyId),
                replyId
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ReplyViewModel>>> GetAllByCommentId(int commentId)
        {
            var replies = (await _replyService.GetAll())
                .Where(r => r.CommentId == commentId).ToList();

            return Ok(_mapper.Map<List<ReplyViewModel>>(replies));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ReplyViewModel>> Post(int bugId, int commentId, AddReplyViewModel replyToBeAdded)
        {
            string userId = _userService.RetrieveUserId();

            var replyAddModel = _mapper.Map<AddReplyModel>(replyToBeAdded);

            replyAddModel.AuthorId = userId;
            replyAddModel.CommentId = commentId;

            var reply = await _replyService.Create(replyAddModel);

            string uri = Url.Action(nameof(Get), "Replies", new { bugId, commentId, replyId = reply.Id })!;

            return Created(uri, reply);
        }

        [HttpDelete("{replyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(int replyId)
        {
            await _replyService.Delete(replyId);

            return Ok();
        }
    }
}

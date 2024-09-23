using AutoMapper;
using Core.DTOs.Replies;
using Core.Other;
using Core.ReplyService;
using Core.UserService;
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
        public async Task<ActionResult<ReplyViewModel>> Get(int replyId)
        {
            var reply = await _replyService.GetById(replyId);

            if (reply is not null)
            {
                return Ok(reply);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<List<ReplyViewModel>>> GetAllByCommentId(int commentId)
        {
            var replies = (await _replyService.GetAll())
                .Where(r => r.CommentId == commentId);

            return Ok(_mapper.Map<List<ReplyViewModel>>(replies));
        }

        [HttpPost]
        public async Task<ActionResult<ReplyViewModel>> Post(int bugId, int commentId, AddReplyViewModel replyToBeAdded)
        {
            string userId = _userService.RetrieveUserId();

            var replyAddModel = _mapper.Map<AddReplyModel>(replyToBeAdded);

            replyAddModel.AuthorId = userId;
            replyAddModel.CommentId = commentId;

            var reply = await _replyService.Create(replyAddModel);

            string uri = Url.Action(nameof(Get), "Replies", new { bugId, commentId, reply.Id })!;

            return Created(uri, reply);
        }
    }
}

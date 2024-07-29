using AutoMapper;
using Core.BugService;
using Core.CommentService;
using Core.DTOs;
using Core.DTOs.Bugs;
using Core.DTOs.Comments;
using Core.Other;
using Core.UserService;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("bugs")]
    [Authorize(Policy = AuthorizePolicy.BasicAccess)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BugsController : BaseController
    {
        private readonly IBugService _bugService;        
        private readonly IUserService<BugUser> _userService;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public BugsController(IBugService bugService, IUserService<BugUser> userService, IMapper mapper, ICommentService commentService)
        {
            this._bugService = bugService;
            this._userService = userService;
            this._mapper = mapper;
            this._commentService = commentService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var bug = await _bugService.GetBugById(id);

            if (bug != null)
            {
                return Ok(_mapper.Map<BugViewModel>(bug));
            }

            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string? pageInput, string? pageSizeInput, string? searchTerm, string? sortOptions, string? filter)
        {
            int page;
            int pageSize;

            if (pageInput == null)
                page = 1;
            else 
                page = int.Parse(pageInput);

            if (pageSizeInput == null)
                pageSize = 50;
            else
                pageSize = int.Parse(pageSizeInput);

            var bugs = await _bugService.GetBugs(page, pageSize, searchTerm, sortOptions, filter);

            return Ok(_mapper.Map<PagedList<BugViewModel>>(bugs));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AddBugViewModel addBugView)
        {
            var newBugModel = _mapper.Map<AddBugModel>(addBugView);

            newBugModel.CreatorId = _userService.RetrieveUserId();

            var bug = await _bugService.CreateBug(newBugModel);

            if (bug != null)
            {
                string uri = Url.Action(nameof(Get), "Bugs", new { bug.Id })!;

                var bugViewModel = _mapper.Map<BugViewModel>(bug);

                return Created(uri, bugViewModel);
            }

            return BadRequest(addBugView);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(EditBugViewModel editBugViewModel)
        {
            if (!editBugViewModel.Validate())
            {
                return BadRequest(new
                {
                    error = "Model invalid",
                    bug = editBugViewModel
                });
            }

            var bug = await _bugService.GetBugById(editBugViewModel.Id);
            
            if (bug != null)
            {
                bug = await _bugService.UpdateBug(editBugViewModel);
                return Ok(_mapper.Map<BugViewModel>(bug));
            }
            else
            {
                return await Post(_mapper.Map<AddBugViewModel>(editBugViewModel));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await _bugService.DeleteBug(id);

            return Ok();
        }

        [HttpGet("{bugId}/comments/{id}")]
        public async Task<IActionResult> GetComment(int bugId, int id)
        {
            var comment = await _commentService.GetById(id);

            if (comment != null)
            {
                if (comment.BugId != bugId)
                {
                    return BadRequest();
                }

                return Ok(_mapper.Map<CommentViewModel>(comment));
            }

            return NotFound();
        }

        [HttpGet("{bugId}/comments")]
        public async Task<IActionResult> GetCommentsByBugId(int bugId)
        {
            var comments = await _commentService.GetCommentsByBugId(bugId);

            return Ok(comments);
        }

        [HttpPost("{bugId}/comments")]
        public async Task<IActionResult> PostComment(int bugId, AddCommentViewModel comment)
        {
            if (!await _bugService.DoesExist(bugId))
            {
                return BadRequest();
            }

            string userId = _userService.RetrieveUserId();

            var newComment = new AddCommentModel()
            {
                AuthorId = userId,
                BugId = bugId,
                Content = comment.Content
            };

            var createdComment = await _commentService.Create(newComment);

            if (createdComment != null)
            {
                string uri = Url.Action(nameof(GetComment), "Bugs", new { bugId = createdComment.BugId, createdComment.Id})!;

                return Created(uri, createdComment);
            }

            return BadRequest();
        }

        [HttpDelete("{bugId}/comments/{id}")]
        public async Task<IActionResult> DeleteCommentById(int bugId, int commentId)
        {
            if (!await _bugService.DoesExist(bugId))
            {
                return BadRequest();
            }

            await _commentService.Delete(commentId);

            return Ok();
        }
    }
}

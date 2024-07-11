using AutoMapper;
using Core.BugService;
using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;
using Core.Other;
using Core.UserService;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("bugs")]
    [Authorize(Policy = AuthorizePolicy.BasicAccess)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BugsController : BaseController
    {
        private readonly IBugService bugService;
        private readonly IMapper mapper;
        private readonly IUserService<BugUser> userService;

        public BugsController(IBugService bugService, IMapper mapper, IUserService<BugUser> userService)
        {
            this.bugService = bugService;
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet("{bugId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BugViewModel>> Get(int bugId)
        {
            var bug = await bugService.RetrieveBug(bugId);

            if (bug == null)
            {
                return NotFound(new
                {
                    error = $"A bug by the requested {bugId} ID has not been found."
                });
            }

            return Ok(bug);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BugViewModel>> Get()
        {
            var bug = await bugService.RetrieveAllActiveBugs();

            if (bug == null)
            {
                return NotFound(new
                {
                    error = "No pending bugs left."
                });
            }

            return Ok(bug);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BugViewModel>> Edit(EditBugViewModel editedBug)
        {
            if (!Enum.TryParse(editedBug.Status, true, out BugStatus _))
            {
                return BadRequest(new
                {
                    error = "Invalid status.",
                    invalidBug = editedBug
                });
            }

            if (!Enum.TryParse(editedBug.Priority, true, out BugPriority _))
            {
                return BadRequest(new
                {
                    error = "Invalid priority.",
                    invalidBug = editedBug
                });
            }

            var bug = await bugService.UpdateOrCreateBug(editedBug);

            return bug;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BugViewModel>> Create(AddBugViewModel newBug)
        {
            if (!Enum.TryParse(newBug.Status, true, out BugStatus _))
            {
                return BadRequest(new
                {
                    error = "Invalid status.",
                    invalidBug = newBug
                });
            }

            if (!Enum.TryParse(newBug.Priority, true, out BugPriority _))
            {
                return BadRequest(new
                {
                    error = "Invalid priority.",
                    invalidBug = newBug
                });
            }

            var inputBug = mapper.Map<AddBugModel>(newBug);

            var userId = userService.RetrieveUserId();

            inputBug.CreatorId = userId;

            var bug = await bugService.AddBug(inputBug);

            if (bug == null)
            {
                return BadRequest(new
                {
                    error = "Could not be added to the list.",
                    invalidBug = newBug
                });
            }

            var uri = Url.Action("Get", "Bugs", new { bugId = bug.Id });

            return Created(uri!, bug);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{bugId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int bugId)
        {
            bool success = await bugService.DeleteBug(bugId);

            if (success)
            {
                return Ok();
            }

            return BadRequest(new
            {
                error = "Could not be deleted."
            });
        }

        [HttpPost("{bugId}/comment")]
        public async Task<IActionResult> AddComment(int bugId, AddCommentViewModel newComment)
        {
            string userId = userService.RetrieveUserId();

            var commentToCreate = mapper.Map<AddCommentModel>(newComment);

            commentToCreate.AuthorId = userId;
            commentToCreate.BugId = bugId;

            var comment = await bugService.AddComment(commentToCreate);

            if (comment == null)
            {
                return BadRequest(new
                {
                    error = "Could not create comment",
                    comment = newComment
                });
            }

            return Ok(comment);
        }

        [HttpPatch("{bugId}/comment/{commentId}/like")]
        public async Task<IActionResult> AdjustLikes(int commentId, char? action)
        {
            var comment = await bugService.EditLikes(commentId, action);

            if (comment != null)
            {
                return Ok(comment);
            }

            return BadRequest();
        }

        [HttpPatch("{bugId}/assign-to")]
        public async Task<IActionResult> AssignBugTo(int bugId, string? userId)
        {
            var bug = await bugService.ReassignBug(bugId, userId); 

            if (bug != null)
            {
                return Ok(bug);
            }

            return BadRequest();
        }
    }
}

using Core.BugService;
using Core.DTOs.Bug;
using Infrastructure.Models.Bug;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("bugs")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BugsController : BaseController
    {
        private readonly IBugService bugService;

        public BugsController(IBugService bugService)
        {
            this.bugService = bugService;
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
            var bug = await bugService.RetrieveAllBugs();

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

            var bug = await bugService.AddBug(newBug);

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
    }
}

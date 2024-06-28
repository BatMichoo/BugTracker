using Core.DbService;
using Core.DTOs.Bug;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ITrackerDbService dbService;

        public BugsController(ITrackerDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpGet("{bugId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BugViewModel>> Get(int bugId)
        {
            var bug = await dbService.GetBug(bugId);

            if (bug == null)
            {
                return NotFound(new
                {
                    error = $"A bug by the requested {bugId} ID has not been found."
                });
            }

            return Ok(bug);
        }

        [HttpPatch]
        public async Task<ActionResult<BugViewModel>> Edit(BugViewModel editedBug)
        {
            var bug = await dbService.EditBug(editedBug);

            return bug;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BugViewModel>> Create(BugViewModel newBug)
        {
            var bug = await dbService.AddBug(newBug);

            if (bug == null)
            {
                return NotFound(new
                {
                    error = "Could not be added to the list."
                });
            }

            var uri = Url.Action("Get", "Bugs", new { bugId = bug.Id });

            return Created(uri, bug);
        }

        [HttpDelete("{bugId}")]
        public async Task<IActionResult> Delete(int bugId)
        {
            bool success = await dbService.DeleteBug(bugId);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}

using AutoMapper;
using Core.BugService;
using Core.DTOs.Bug;
using Core.DTOs.Comment;
using Core.Models.Bug.BugEnums;
using Core.Other;
using Core.Repository;
using Core.UserService;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("bugs")]
    [Authorize(Policy = AuthorizePolicy.BasicAccess)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BugsController : BaseController
    {
        private readonly IBugService bugService;        
        private readonly IUserService<BugUser> userService;

        public BugsController(IBugService bugService, IUserService<BugUser> userService)
        {
            this.bugService = bugService;
            this.userService = userService;
        }

        [HttpGet("{bugId}")]
        public async Task<IActionResult> Get(int bugId)
        {
            var bug = await bugService.FetchBugById(bugId);

            if (bug != null)
            {
                return Ok(bug);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddBugViewModel addBugView)
        {
            var bug = await bugService.CreateBug(new AddBugModel()
            {
                CreatorId = userService.RetrieveUserId(),
                Description = addBugView.Description,
                Priority = addBugView.Priority,
                Status = addBugView.Status,
            });

            if (bug != null)
            {
                string uri = Url.Action("Get", "Bugs", new { bug.Id });
                return Created(uri, new BugViewModel
                {
                    Id = bug.Id,
                    AssignedTo = bug.Assignee,
                    CreatedOn = bug.CreatedOn,
                    LastUpdatedOn = bug.LastUpdatedOn,
                    Status = bug.Status,
                    Priority = bug.Priority,
                    Description = bug.Description,
                    CreatedBy = bug.Creator
                });
            }

            return BadRequest(addBugView);
        }

        [HttpGet]
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

            var bugs = await bugService.FetchBugs(page, pageSize, searchTerm, sortOptions, filter);

            return Ok(bugs);
        }        
    }
}

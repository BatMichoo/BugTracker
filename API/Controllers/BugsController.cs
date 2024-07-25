using AutoMapper;
using Core.BugService;
using Core.DTOs;
using Core.DTOs.Bug;
using Core.Other;
using Core.UserService;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("bugs")]
    [Authorize(Policy = AuthorizePolicy.BasicAccess)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BugsController : BaseController
    {
        private readonly IBugService bugService;        
        private readonly IUserService<BugUser> userService;
        private readonly IMapper mapper;

        public BugsController(IBugService bugService, IUserService<BugUser> userService, IMapper mapper)
        {
            this.bugService = bugService;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var bug = await bugService.FetchBugById(id);

            if (bug != null)
            {
                return Ok(mapper.Map<BugViewModel>(bug));
            }

            return BadRequest();
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

            var bugs = await bugService.FetchBugs(page, pageSize, searchTerm, sortOptions, filter);

            return Ok(mapper.Map<PagedList<BugViewModel>>(bugs));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AddBugViewModel addBugView)
        {
            var newBugModel = mapper.Map<AddBugModel>(addBugView);

            newBugModel.CreatorId = userService.RetrieveUserId();

            var bug = await bugService.CreateBug(newBugModel);

            if (bug != null)
            {
                string uri = Url.Action("Get", "Bugs", new { bug.Id })!;

                var bugViewModel = mapper.Map<BugViewModel>(bug);

                return Created(uri, bugViewModel);
            }

            return BadRequest(addBugView);
        }

        [HttpPut("{id}")]
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
            var bug = await bugService.UpdateBug(editBugViewModel);

            if (bug != null)
            {
                return Ok(mapper.Map<BugViewModel>(bug));
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await bugService.DeleteBug(id);

            return Ok();
        }
    }
}

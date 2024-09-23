using AutoMapper;
using Core.BugService;
using Core.DTOs;
using Core.DTOs.Bugs;
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

        public BugsController(IBugService bugService, IUserService<BugUser> userService, IMapper mapper)
        {
            _bugService = bugService;
            _userService = userService;
            _mapper = mapper;           
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            var bug = await _bugService.GetById(id);

            if (bug is not null)
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
            {
                page = 1;
            }
            else
            {
                page = int.Parse(pageInput);
            }

            if (pageSizeInput == null)
            {
                pageSize = 50;
            }
            else
            {
                pageSize = int.Parse(pageSizeInput);
            }

            var bugs = await _bugService.Fetch(page, pageSize, searchTerm, sortOptions, filter);

            return Ok(_mapper.Map<PagedList<BugViewModel>>(bugs));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(AddBugViewModel addBugView)
        {
            var newBugModel = _mapper.Map<AddBugModel>(addBugView);

            newBugModel.CreatorId = _userService.RetrieveUserId();

            var bug = await _bugService.Create(newBugModel);

            if (bug is not null)
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

            bool doesBugExist = await _bugService.DoesExist(editBugViewModel.Id);

            if (doesBugExist)
            {
                var editModel = _mapper.Map<EditBugModel>(editBugViewModel);

                string userId = _userService.RetrieveUserId();

                editModel.LastUpdatedById = userId;
                
                var updatedModel = await _bugService.Update(editModel);

                return Ok(_mapper.Map<BugViewModel>(updatedModel));
            }

            return await Post(_mapper.Map<AddBugViewModel>(editBugViewModel));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await _bugService.Delete(id);

            return Ok();
        }

        [HttpGet("assigned-to/{userId}")]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignedBugs(string userId)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    error = "User does not exist",
                    userId
                });
            }

            var userWithBugs = await _bugService.GetAssignedToUserId(userId);

            return Ok(userWithBugs);
        }

        [HttpGet("created-by/{userId}")]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCreatedByBugs(string userId)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    error = "User does not exist.",
                    userId
                });
            }

            var userWithBugs = await _bugService.GetCreatedByUserId(userId);

            return Ok(userWithBugs);
        }
    }
}

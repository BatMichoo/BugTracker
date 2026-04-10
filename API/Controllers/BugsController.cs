using API.Utilities.ErrorMessages;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.Bugs;
using Core.Entities.UserEntity;
using Core.EntitiesQueryUtilities;
using Core.EntitiesQueryUtilities.QueryParameters.Bugs;
using Core.Models.Bugs.BugEnums;
using Core.Other;
using Core.Services.BugService;
using Core.Services.UserService;
using Core.Repositories;
using Core.Entities.NotifEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [Route("/bugs")]
    [Authorize(Policy = AuthorizePolicy.UserAccess)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class BugsController : BaseController
    {
        private readonly IBugService _bugService;
        private readonly IUserService<BugUser> _userService;
        private readonly IMapper _mapper;
        private readonly IBugQueryParametersFactory _queryFactory;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IBugNotificationRepository _notifRepo;
        private readonly IMemoryCache _cache;
        private const string DefaultBugsCacheKey = "BugsDefault";

        public BugsController(IBugNotificationRepository notifRepository, IHubContext<NotificationHub> notificationHub, IBugService bugService, IUserService<BugUser> userService, IMapper mapper, IBugQueryParametersFactory queryFactory, IMemoryCache cache)
        {
            _notificationHub = notificationHub;
            _bugService = bugService;
            _userService = userService;
            _mapper = mapper;
            _queryFactory = queryFactory;
            _notifRepo = notifRepository;
            _cache = cache;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BugViewModel>> Get(int id, bool isFullyIncluded)
        {
            var bug = await _bugService.GetById(id, isFullyIncluded);

            if (bug is not null)
            {
                return Ok(_mapper.Map<BugViewModel>(bug));
            }

            return NotFound(new
            {
                errorMessage = string.Format(ErrorMessage.Bugs.NotFound, id),
                id
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string? searchTerm, string? sortOptions, string? filter,
            int pageInput = PagingDefaults.StartingPageNumber, int pageSizeInput = PagingDefaults.ElementsPerPage)
        {
            // Check if request is for the default, parameterless state
            bool isDefaultRequest = string.IsNullOrEmpty(searchTerm) &&
                                    string.IsNullOrEmpty(sortOptions) &&
                                    string.IsNullOrEmpty(filter) &&
                                    pageInput == PagingDefaults.StartingPageNumber &&
                                    pageSizeInput == PagingDefaults.ElementsPerPage;

            if (isDefaultRequest && _cache.TryGetValue(DefaultBugsCacheKey, out QueryViewModel<BugViewModel>? cachedResponse))
            {
                return Ok(cachedResponse);
            }

            var queryParameters = await _queryFactory.ProcessQueryParametersInput(pageInput, pageSizeInput, searchTerm, sortOptions, filter);
            var bugs = await _bugService.Fetch(queryParameters);
            var response = _mapper.Map<QueryViewModel<BugViewModel>>(bugs);

            if (isDefaultRequest)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

                _cache.Set(DefaultBugsCacheKey, response, cacheOptions);
            }

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BugViewModel>> Post(AddBugViewModel addBugView)
        {
            var newBugModel = _mapper.Map<AddBugModel>(addBugView);

            newBugModel.CreatorId = _userService.RetrieveUserId();

            var bug = await _bugService.Create(newBugModel);

            if (bug is not null)
            {
                string uri = Url.Action(nameof(Get), "Bugs", new { bug.Id })!;

                var bugViewModel = _mapper.Map<BugViewModel>(bug);

                if (!string.IsNullOrWhiteSpace(bugViewModel.AssignedTo?.Id) && bugViewModel.AssignedTo?.Id != newBugModel.CreatorId)
                {
                    var notification = new BugNotification
                    {
                        BugId = bugViewModel.Id,
                        AssignedById = newBugModel.CreatorId,
                        AssigneeId = bugViewModel.AssignedTo.Id
                    };

                    await _notifRepo.Create(notification);

                    await _notificationHub.Clients.User(bugViewModel.AssignedTo.Id).SendAsync("new-assigned-bug", notification);
                }

                _cache.Remove(DefaultBugsCacheKey);

                return Created(uri, bugViewModel);
            }

            return BadRequest(new
            {
                errorMessage = ErrorMessage.Bugs.UnableToCreate,
                addBugView
            });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BugViewModel>> Put(EditBugViewModel editBugViewModel)
        {
            if (!editBugViewModel.Validate())
            {
                return BadRequest(new
                {
                    errorMessage = ErrorMessage.Bugs.InvalidUpdateModel,
                    model = editBugViewModel
                });
            }

            bool doesBugExist = await _bugService.DoesExist(editBugViewModel.Id);

            if (doesBugExist)
            {
                var editModel = _mapper.Map<EditBugModel>(editBugViewModel);

                string userId = _userService.RetrieveUserId();

                editModel.LastUpdatedById = userId;
                editModel.LastUpdatedOn = DateTime.Now;

                string? oldAssigneeId = await _bugService.GetAssigneeId(editBugViewModel.Id);

                var updatedModel = await _bugService.Update(editModel);

                _cache.Remove(DefaultBugsCacheKey);

                if (!string.IsNullOrWhiteSpace(editBugViewModel.AssigneeId) && oldAssigneeId != editBugViewModel.AssigneeId
                        && editBugViewModel.AssigneeId != userId)
                {
                    var notification = new BugNotification
                    {
                        BugId = editBugViewModel.Id,
                        AssignedById = userId,
                        AssigneeId = editBugViewModel.AssigneeId
                    };

                    await _notifRepo.Create(notification);

                    await _notificationHub.Clients.User(editBugViewModel.AssigneeId).SendAsync("new-assigned-bug", notification);
                }

                return Ok(_mapper.Map<BugViewModel>(updatedModel));
            }

            return await Post(_mapper.Map<AddBugViewModel>(editBugViewModel));
        }

        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            await _bugService.Delete(id);

            _cache.Remove(DefaultBugsCacheKey);

            return Ok();
        }

        [HttpGet("assigned-to/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QueryViewModel<BugViewModel>>> GetAssignedBugs(string userId)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    errorMessage = string.Format(ErrorMessage.Users.NotFound, userId),
                    userId
                });
            }

            var queryParameters = _queryFactory.CreateAssignedToUserQuery(userId);

            var userWithBugs = await _bugService.Fetch(queryParameters);

            var models = _mapper.Map<QueryViewModel<BugViewModel>>(userWithBugs);

            return Ok(models);
        }

        [HttpGet("created-by/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QueryViewModel<BugViewModel>>> GetCreatedByBugs(string userId)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    errorMessage = string.Format(ErrorMessage.Users.NotFound, userId),
                    userId
                });
            }

            var queryParameters = _queryFactory.CreateMadeByUserQuery(userId);

            var userWithBugs = await _bugService.Fetch(queryParameters);

            var models = _mapper.Map<QueryViewModel<BugViewModel>>(userWithBugs);

            return Ok(models);
        }

        [HttpGet("close/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Close(int id)
        {
            var bug = await _bugService.GetById(id);

            if (bug is not null)
            {
                if (bug.Status != BugStatus.Fixed)
                {
                    bug.Status = BugStatus.Fixed;

                    bug = await _bugService.Update(_mapper.Map<EditBugModel>(bug));

                    return Ok();
                }

                return BadRequest();
            }

            return NotFound(new
            {
                errorMessage = string.Format(ErrorMessage.Bugs.NotFound, id),
                id
            });
        }
    }
}

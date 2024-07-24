using AutoMapper;
using Core.BugService;
using Core.DTOs.User;
using Core.Other;
using Core.UserService;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("users")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UsersController : BaseController
    {
        private readonly IUserService<BugUser> userService;
        private readonly IBugService bugService;
        private readonly IMapper mapper;

        public UsersController(IUserService<BugUser> userService, IBugService bugService, IMapper mapper)
        {
            this.userService = userService;
            this.bugService = bugService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn(LoginUserModel loginUserModel)
        {
            var user = await userService.RetrieveUserByEmail(loginUserModel.Email);

            if (user != null)
            {
                var result = await userService.SignInUserWithPassword(user, loginUserModel.Password);

                if (result)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserModel newUser)
        {
            var user = await userService.RetrieveUserByEmail(newUser.Email);

            if (user == null)
            {
                try
                {
                    user = await userService.RegisterNewUserWithPassword(newUser);
                }
                catch (ArgumentException ex) 
                {
                    return BadRequest(ex.Message);
                }
            }

            string uri = Url.Action("GetUser", "Users", new { userId = user.Id })!;

            return Created(uri, mapper.Map<UserViewModel>(user));
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogOut()
        {
            await userService.SignOut();

            return Ok();
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await userService.RetrieveUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> RetrieveUserList()
        {
            var users = await userService.RetrieveUserList();

            return Ok(users);
        }

        [HttpGet("roles")]
        [Authorize(Policy = AuthorizePolicy.ElevatedAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveRoles()
        {
            var roles = await userService.GetRoles();

            return Ok(roles);
        }

        [HttpPatch("assign-role")]
        [Authorize(Policy = AuthorizePolicy.ElevatedAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await userService.RetrieveUserById(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    error = "User not found.",
                    user
                });
            }

            if (role == UserRoles.User || role == UserRoles.Manager)
            {
                var success = await userService.AddRolesToUser(user, new List<string> { role });

                if (success)
                {
                    return Ok();
                }
            }

            return BadRequest(new
            {
                error = "Role assignment failed",
                role
            });
        }

        [HttpGet("{userId}/assigned-bugs")]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignedBugs(string userId)
        {
            var user = await userService.RetrieveUserById(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    error = "User does not exist",
                    userId
                });
            }

            var userWithBugs = await bugService.FetchBugById(int.Parse(userId));

            return Ok(userWithBugs);
        }

        [HttpGet("{userId}/created-bugs")]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCreatedBugs(string userId)
        {
            var user = await userService.RetrieveUserById(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    error = "User does not exist.",
                    userId
                });
            }

            var userWithBugs = await bugService.FetchBugById(int.Parse(userId));

            return Ok(userWithBugs);
        }
    }
}

using AutoMapper;
using Core.BugService;
using Core.DTOs.Users;
using Core.Other;
using Core.UserService;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("users")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UsersController : BaseController
    {
        private readonly IUserService<BugUser> _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService<BugUser> userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn(LoginUserModel loginUserModel)
        {
            var user = await _userService.RetrieveUserByEmail(loginUserModel.Email);

            if (user is not null)
            {
                var result = await _userService.SignInUserWithPassword(user, loginUserModel.Password);

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
            var user = await _userService.RetrieveUserByEmail(newUser.Email);

            if (user is null)
            {
                try
                {
                    user = await _userService.RegisterNewUserWithPassword(newUser);
                }
                catch (ArgumentException ex) 
                {
                    return BadRequest(ex.Message);
                }
            }

            string uri = Url.Action("GetUser", "Users", new { userId = user.Id })!;

            return Created(uri, _mapper.Map<UserViewModel>(user));
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOut();

            return Ok();
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.BasicAccess)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> RetrieveUserList()
        {
            var users = await _userService.RetrieveUserList();

            return Ok(users);
        }

        [HttpGet("roles")]
        [Authorize(Policy = AuthorizePolicy.ElevatedAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveRoles()
        {
            var roles = await _userService.GetRoles();

            return Ok(roles);
        }

        [HttpPatch("assign-role")]
        [Authorize(Policy = AuthorizePolicy.ElevatedAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    error = "User not found.",
                    user
                });
            }

            if (role == UserRoles.User || role == UserRoles.Manager)
            {
                var success = await _userService.AddRolesToUser(user, new List<string> { role });

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
    }
}

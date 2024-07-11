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

        public UsersController(IUserService<BugUser> userService)
        {
            this.userService = userService;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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

            return Ok();
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogOut()
        {
            await userService.SignOut();

            return Ok();
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
    }
}

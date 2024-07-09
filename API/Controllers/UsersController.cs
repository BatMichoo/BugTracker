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
            var user = await userService.RetrieveUser();

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
        [Authorize(Policy = AuthorizePolicy.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveUserList()
        {
            var users = await userService.RetrieveUserList();

            return Ok(users);
        }
    }
}

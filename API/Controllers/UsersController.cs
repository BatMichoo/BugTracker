using Core.DTOs.User;
using Core.UserService;
using Infrastructure.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("users")]
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly UserManager<BugUser> _userManager;
        private readonly SignInManager<BugUser> _signInManager;

        public UsersController(IUserService userService, UserManager<BugUser> userManager, SignInManager<BugUser> signInManager)
        {
            this.userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn(LoginUserModel loginUserModel)
        {
            var user = await _userManager.FindByEmailAsync(loginUserModel.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginUserModel.Password, false, false);

                if (result.Succeeded)
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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                var toBeCreated = new BugUser()
                {
                    Email = newUser.Email,
                    Name = newUser.FirstName + newUser.LastName,
                    UserName = newUser.UserName
                };

                var result = await _userManager.CreateAsync(toBeCreated, newUser.Password);

                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(toBeCreated, newUser.Password, false, false);

                    return Created(nameof(LogIn), newUser);
                }

                return BadRequest(result.Errors);
            }

            return BadRequest();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                await _signInManager.SignOutAsync();

                return Ok();
            }

            return BadRequest(new
            {
                error = "No user to sign out."
            });
        }
    }
}

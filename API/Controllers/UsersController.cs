using API.Utilities.ErrorMessages;
using AutoMapper;
using Core.DTOs.Roles;
using Core.DTOs.Searches;
using Core.DTOs.Users;
using Core.Entities.CustomRole;
using Core.Entities.SearchEntity;
using Core.Entities.UserEntity;
using Core.Other;
using Core.Services.SearchesService;
using Core.Services.UserService;
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
        private readonly ISearchesService _searchesService;

        public UsersController(IUserService<BugUser> userService, IMapper mapper, ISearchesService searchesService)
        {
            _userService = userService;
            _mapper = mapper;
            _searchesService = searchesService;
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
                    var loginResponse = await _userService.GenerateLoginResponse(user);

                    return Ok(loginResponse);
                }
            }

            return BadRequest(new
            {
                errorMessage = ErrorMessage.Users.LoginFailed
            });
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
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string userId)
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

            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> RetrieveUserList()
        {
            var users = await _userService.RetrieveUserList();

            return Ok(users);
        }

        [HttpGet("roles")]
        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveRoles()
        {
            var roles = await _userService.GetAllUserRoles();

            return Ok(roles);
        }

        [HttpPost("roles")]
        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var newRole = new CustomRole(roleName, isDeletable: true);
            CustomRole role = await _userService.CreateRole(newRole);
            var roleView = new RoleView { Name = role.Name, IsDeletable = role.IsDeletable };

            return Ok(roleView);
        }

        [HttpDelete("roles")]
        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _userService.GetRoleByName(roleName);
            bool success = await _userService.DeleteRole(role);

            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPatch("assign-role")]
        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
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
                    errorMessage = string.Format(ErrorMessage.Users.NotFound, userId),
                    user
                });
            }

            var success = await _userService.AddRolesToUser(user, new List<string> { role });

            if (success)
            {
                return Ok();
            }

            return BadRequest(new
            {
                errorMessage = string.Format(ErrorMessage.Users.CouldNotAssignRole, role),
                role
            });
        }

        [HttpPatch("unassign-role")]
        [Authorize(Policy = AuthorizePolicy.ManagerAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnassignRole(string userId, string role)
        {
            var user = await _userService.RetrieveUserById(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    errorMessage = string.Format(ErrorMessage.Users.NotFound, userId),
                    user
                });
            }

            var success = await _userService.RemoveRolesFromUser(user, new List<string> { role });

            if (success)
            {
                return Ok();
            }

            return BadRequest(new
            {
                errorMessage = string.Format(ErrorMessage.Users.CouldNotAssignRole, role),
                role
            });
        }

        [HttpGet("profile")]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProfile(string userId)
        {
            return Ok();
        }

        [HttpPost("change-password")]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userService.RetrieveUser();

            try
            {
                bool success = await _userService.ChangePassword(user, model.OldPassword, model.NewPassword);
                if (success)
                {
                    return Ok("Password change successful!");
                }

                return BadRequest("Couldn't change password.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("searches")]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSavedSearches()
        {
            string userId = _userService.RetrieveUserId();

            var searches = await _searchesService.GetForUser(userId);

            return Ok(searches);
        }

        [HttpGet("searches/{id}")]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSavedSearch(int id)
        {
            var search = await _searchesService.GetById(id);

            return Ok(search);
        }

        [HttpPost("searches")]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSavedSearch(CreateSearchModel createModel)
        {
            string userId = _userService.RetrieveUserId();

            var search = new Search
            {
                Name = createModel.Name,
                CreatedById = userId,
                QueryString = createModel.QueryString
            };

            var createdSearch = await _searchesService.Create(search);

            string uri = Url.Action(nameof(GetSavedSearch), "Users", new { createdSearch.Id })!;

            return Created(uri, createdSearch);
        }

        [HttpDelete("searches/{id}")]
        [Authorize(Policy = AuthorizePolicy.UserAccess)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSavedSearch(int id)
        {
            await _searchesService.Delete(id);

            return Ok();
        }
    }
}

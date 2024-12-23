using AutoMapper;
using Core.DTOs.Users;
using Core.Other;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services.UserService
{
    public class UserService<T> : IUserService<T> where T : BugUser
    {
        private readonly UserManager<T> _userManager;
        private readonly SignInManager<T> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public UserService(UserManager<T> userManager, SignInManager<T> signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
            _roleManager = roleManager;
        }

        public string RetrieveUserId()
        {
            return _userManager.GetUserId(_claimsPrincipal);
        }

        public async Task<T> RegisterNewUserWithPassword(RegisterUserModel newUser)
        {
            var toBeCreated = _mapper.Map<T>(newUser);

            var result = await _userManager.CreateAsync(toBeCreated, newUser.Password);

            if (result.Succeeded)
            {
                var roleSucceeded = await AddRolesToUser(toBeCreated, new List<string>() { UserRoles.User });

                if (roleSucceeded)
                {
                    return toBeCreated;
                }

                throw new ArgumentException("Roles could not be added.");
            }

            throw new ArgumentException(string.Join(Environment.NewLine, result.Errors));
        }

        public async Task<T> RetrieveUser()
        {
            var user = await _userManager.GetUserAsync(_claimsPrincipal);

            return user;
        }

        public async Task<T> RetrieveUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<bool> SignInUserWithPassword(T user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            return result.Succeeded;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> AddRolesToUser(T user, IEnumerable<string> roles)
        {
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result.Succeeded;
        }

        public async Task<bool> RemoveRolesFromUser(T user, IEnumerable<string> roles)
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            return result.Succeeded;
        }

        public async Task<List<UserViewModel>> RetrieveUserList()
        {
            var users = await _userManager.Users
                .Where(u => u.UserName != "Administrator")
                .AsNoTracking()
                .ToListAsync();

            if (users.Any())
            {
                return _mapper.Map<List<UserViewModel>>(users);
            }

            return new List<UserViewModel>();
        }

        public async Task<List<string>> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .Where(r => r.Name != UserRoles.Admin)
                .Select(r => r.Name)
                .ToListAsync();

            if (roles.Any())
            {
                return roles;
            }

            return new List<string>();
        }

        public async Task<T> RetrieveUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<LoginResponseModel> GenerateLoginResponse(T user)
        {
            var userRoles = await GetAllRoles();

            var roleClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r));

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Name, user.Name)
            };

            claims.AddRange(roleClaims);

            string secretKey = Environment.GetEnvironmentVariable("JwtSecretKey")!;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7272",
                audience: "https://localhost:7094",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseModel()
            {
                Token = tokenString,
            };
        }
    }
}

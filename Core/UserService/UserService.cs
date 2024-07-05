using Core.DTOs.User;
using Infrastructure;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.UserService
{
    public class UserService : IUserService
    {
        private readonly TrackerDbContext dbContext;

        public UserService(TrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> RegisterUser(RegisterUserModel user, ClaimsIdentity identity)
        {
            var userEmail = identity.Claims.FirstOrDefault(c => c.Type == "email");

            var newUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (newUser == null)
            {
                newUser = new BugUser()
                {
                    Email = user.Email,
                    UserName = $"{user.FirstName}{user.LastName}"
                };

                await dbContext.Users.AddAsync(newUser);

                await dbContext.SaveChangesAsync();
            }

            var emailClaim = new Claim(ClaimTypes.Email, user.Email);
            var usernameClaim = new Claim(ClaimTypes.Name, newUser.UserName);
            var idClaim = new Claim(ClaimTypes.NameIdentifier, newUser.Id);

            List<Claim> claims = new List<Claim>() { emailClaim, usernameClaim, idClaim };

            identity.AddClaims(claims);

            return true;
        }
    }
}

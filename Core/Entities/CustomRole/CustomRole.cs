using Microsoft.AspNetCore.Identity;

namespace Core.Entities.CustomRole
{
    public class CustomRole : IdentityRole
    {
        public CustomRole() : base() {
        }
        public CustomRole(string roleName, bool isDeletable) : base(roleName)
        {
            IsDeletable = isDeletable;
        }

        public bool IsDeletable { get; set; }
    }
}

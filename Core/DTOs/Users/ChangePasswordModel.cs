namespace Core.DTOs.Users
{
    public class ChangePasswordModel
    {
        public string Id { get; set; } = null!;
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

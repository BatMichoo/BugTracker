namespace Core.Entities.NotifEntity
{
    public class BugNotification : BaseModel
    {
        public string AssignedById { get; set; } = null!;
        public string AssigneeId { get; set; } = null!;
        public int BugId { get; set; }
        public bool IsRead { get; set; }
    }

    public class RoleNotification : BaseModel
    {
        public string AssignedById { get; set; } = null!;
        public string AssigneeId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool IsRead { get; set; }
    }
}

namespace Core.DTOs.Bugs
{
    public class EditBugModel
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? Description { get; set; }
        public string? AssigneeId { get; set; }
    }
}

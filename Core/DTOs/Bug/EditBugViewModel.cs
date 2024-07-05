namespace Core.DTOs.Bug
{
    public class EditBugViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}

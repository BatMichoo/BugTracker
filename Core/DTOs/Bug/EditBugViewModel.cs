using Infrastructure.Models.Bug;
namespace Core.DTOs.Bug
{
    public class EditBugViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
    }
}

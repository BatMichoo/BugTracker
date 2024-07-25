namespace Core.DTOs.Bug
{
    public class EditBugViewModel
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; } 
        public string? Description { get; set; }
        public string? AssigneeId { get; set; } 

        public bool Validate()
        {
            bool hasStatus = !string.IsNullOrEmpty(Status);
            bool hasPriority = !string.IsNullOrEmpty(Priority);
            bool hasDescription = !string.IsNullOrEmpty(Description);
            bool isAssigned = !string.IsNullOrEmpty(AssigneeId);

            return hasStatus || hasPriority || hasDescription || isAssigned;
        }
    }
}

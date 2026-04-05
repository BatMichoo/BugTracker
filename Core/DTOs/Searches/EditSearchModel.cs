namespace Core.DTOs.Searches
{
    public class EditSearchModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string QueryString { get; set; } = null!;
    }
}

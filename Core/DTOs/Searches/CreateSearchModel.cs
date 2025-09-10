namespace Core.DTOs.Searches
{
    public class CreateSearchModel()
    {
        public string Name { get; set; } = null!;
        public string QueryString { get; set; } = null!;
    }
}

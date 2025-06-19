
using Core.Entities.UserEntity;

namespace Core.Entities.SearchEntity
{
    public class Search : BaseModel
    {
        public string Name {get; set;} = null!;
        public string QueryString { get; set; } = null!;
        public string CreatedById { get; set; } = null!;
        public BugUser CreatedBy { get; set; } = null!;
    }
}


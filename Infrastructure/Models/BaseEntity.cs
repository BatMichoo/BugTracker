using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}

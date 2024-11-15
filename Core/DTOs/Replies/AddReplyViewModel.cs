using Infrastructure.Models.CommentEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Replies
{
    public class AddReplyViewModel
    {
        [MaxLength(CommentValidation.MaxLength)]
        public string Content { get; set; } = null!;
    }
}

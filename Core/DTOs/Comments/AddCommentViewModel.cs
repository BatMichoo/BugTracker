using Core.Entities.CommentEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Comments
{
    public class AddCommentViewModel
    {
        [MaxLength(CommentValidation.MaxLength)]
        public string Content { get; set; } = null!;
    }
}

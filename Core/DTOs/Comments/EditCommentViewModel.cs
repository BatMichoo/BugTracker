using Core.Entities.CommentEntity;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Comments
{
    public class EditCommentViewModel
    {
        public int Id { get; set; }

        [MaxLength(CommentValidation.MaxLength)]
        public string Content { get; set; } = null!;
    }
}

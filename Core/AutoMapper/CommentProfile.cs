using AutoMapper;
using Core.DTOs.Comment;
using Infrastructure.Models.Comment;

namespace Core.AutoMapper
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentViewModel>();

            CreateMap<CommentModel, Comment>()
                .ReverseMap();

            CreateMap<CommentModel, CommentViewModel>()
                .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.UserName));

            CreateMap<AddCommentViewModel, AddCommentModel>();

            CreateMap<AddCommentModel, CommentModel>()
                .ForMember(d => d.PostedOn, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<EditCommentViewModel, EditCommentModel>()
                .ReverseMap();

            CreateMap<EditCommentModel, CommentModel>()
                .ForMember(d => d.Author, opt => opt.Ignore())
                .ForMember(d => d.AuthorId, opt => opt.Ignore());
        }
    }
}

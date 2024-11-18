using AutoMapper;
using Core.DTOs;
using Core.DTOs.Comments;
using Infrastructure.Models.CommentEntity;

namespace Core.AutoMapper
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<PagedList<CommentModel>, PagedList<CommentViewModel>>()
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));

            CreateMap<CommentModel, Comment>()
                .ReverseMap();

            CreateMap<CommentModel, CommentViewModel>()
                .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.UserName));

            CreateMap<AddCommentViewModel, AddCommentModel>();

            CreateMap<AddCommentModel, Comment>()
                .ForMember(d => d.PostedOn, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<EditCommentViewModel, EditCommentModel>()
                .ReverseMap();

            CreateMap<EditCommentViewModel, AddCommentViewModel>();

            CreateMap<EditCommentModel, Comment>()
                .ForMember(d => d.LastUpdatedOn, opt => opt.MapFrom(s => DateTime.Now))
                .ForMember(d => d.Author, opt => opt.Ignore())
                .ForMember(d => d.AuthorId, opt => opt.Ignore())
                .ForMember(d => d.Likes, opt => opt.Ignore());
        }
    }
}

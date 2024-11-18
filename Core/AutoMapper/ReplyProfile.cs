using AutoMapper;
using Core.DTOs.Replies;
using Infrastructure.Models.ReplyEntity;

namespace Core.AutoMapper
{
    public class ReplyProfile : Profile
    {
        public ReplyProfile()
        {
            CreateMap<Reply, ReplyModel>();

            CreateMap<ReplyModel, ReplyViewModel>()
                .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.Name));

            CreateMap<AddReplyViewModel, AddReplyModel>()
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<AddReplyModel, Reply>();
        }
    }
}

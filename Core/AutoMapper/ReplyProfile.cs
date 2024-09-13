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

            CreateMap<ReplyModel, ReplyViewModel>();

            CreateMap<AddReplyViewModel, AddReplyModel>()
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<AddReplyModel, Reply>();
        }
    }
}

using AutoMapper;
using Core.DTOs.Bug;
using Infrastructure.Models.Bug;

namespace API.AutoMapper
{
    public class BugProfile : Profile
    {
        public BugProfile()
        {
            CreateMap<Bug, BugModel>()
                .ReverseMap();

            CreateMap<BugModel, BugViewModel>()
                .ReverseMap();

            CreateMap<Bug, BugViewModel>()
                .ReverseMap();
        }
    }
}

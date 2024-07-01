using AutoMapper;
using Core.DTOs.Bug;
using Infrastructure.Models.Bug;

namespace API.AutoMapper
{
    public class BugProfile : Profile
    {
        public BugProfile()
        {
            CreateMap<AddBugViewModel, BugModel>()
                //.ForMember(d => d.Status, opt => opt.MapFrom(s => Enum.Parse<BugStatus>(s.Status)))
                //.ForMember(d => d.Priority, opt => opt.MapFrom(s => Enum.Parse<BugPriority>(s.Priority)))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
                .AfterMap((s, d) => 
                {
                    d.CreatedOn = DateTime.Now;
                    d.LastUpdatedOn = d.CreatedOn;
                });

            CreateMap<EditBugViewModel, BugModel>()
                .ForMember(s => s.LastUpdatedOn, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<Bug, BugModel>()
                .ReverseMap();

            CreateMap<BugModel, BugViewModel>()
                .ReverseMap();

            CreateMap<Bug, BugViewModel>()
                .ReverseMap();
        }
    }
}

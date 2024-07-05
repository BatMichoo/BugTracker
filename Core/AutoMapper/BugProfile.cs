using AutoMapper;
using Core.DTOs.Bug;
using Infrastructure.Models.Bug;

namespace API.AutoMapper
{
    public class BugProfile : Profile
    {
        public BugProfile()
        {
            CreateMap<AddBugViewModel, AddBugModel>()
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
                .AfterMap((s, d) => 
                {
                    d.CreatedOn = DateTime.Now;
                    d.LastUpdatedOn = d.CreatedOn;
                });

            CreateMap<AddBugModel, BugModel>()
                .ForMember(d => d.LastUpdatedById, opt => opt.MapFrom(s => s.CreatorId));

            CreateMap<BugModel, Bug>();

            CreateMap<EditBugViewModel, BugModel>()
                .ForMember(s => s.LastUpdatedOn, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<Bug, BugModel>()
                .ForMember(d => d.Creator, opt => opt.MapFrom(s => s.Creator.UserName))
                .ForMember(d => d.Assignee, opt => opt.MapFrom(s => s.Assignee.Name))
                .ForMember(d => d.LastUpdatedBy, opt => opt.MapFrom(s => s.LastUpdatedBy.UserName));

            CreateMap<BugModel, BugViewModel>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Creator))
                .ForMember(d => d.AssignedTo, opt => opt.MapFrom(s => s.Assignee));

            CreateMap<Bug, BugViewModel>()
                .ReverseMap();
        }
    }
}

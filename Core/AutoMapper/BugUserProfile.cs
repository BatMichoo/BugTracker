using AutoMapper;
using Core.DTOs.User;
using Infrastructure.Models.User;

namespace Core.AutoMapper
{
    public class BugUserProfile : Profile
    {
        public BugUserProfile()
        {
            CreateMap<RegisterUserModel, BugUser>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName));
        }
    }
}

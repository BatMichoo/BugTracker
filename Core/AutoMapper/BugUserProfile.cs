using AutoMapper;
using Core.DTOs.Users;
using Core.Entities.UserEntity;

namespace Core.AutoMapper
{
    public class BugUserProfile : Profile
    {
        public BugUserProfile()
        {
            CreateMap<RegisterUserModel, BugUser>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName));

            CreateMap<BugUser, UserModel>();

            CreateMap<UserModel, UserViewModel>();

            CreateMap<BugUser, UserViewModel>();

            CreateMap<BugUser, UserAssignedBugsModel>();

            CreateMap<BugUser, UserCreatedBugsModel>();

            CreateMap<UserAssignedBugsModel, UserAssignedBugsViewModel>();

            CreateMap<UserCreatedBugsModel, UserCreatedBugsViewModel>();
        }
    }
}

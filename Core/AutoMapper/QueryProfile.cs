using AutoMapper;
using Core.DTOs;
using Core.DTOs.Bugs;

namespace Core.AutoMapper
{
    public class QueryProfile : Profile
    {
        public QueryProfile()
        {
            CreateMap<QueryModel<BugModel>, QueryViewModel<BugViewModel>>();

            CreateMap<FilterModel, FilterViewModel>();
            CreateMap<SortingModel, SortingViewModel>();
        }
    }
}

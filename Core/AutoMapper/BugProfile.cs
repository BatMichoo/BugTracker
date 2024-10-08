﻿using AutoMapper;
using Core.DTOs;
using Core.DTOs.Bugs;
using Infrastructure.Models.BugEntity;

namespace API.AutoMapper
{
    public class BugProfile : Profile
    {
        public BugProfile()
        {
            CreateMap<PagedList<BugModel>, PagedList<BugViewModel>>()
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));
            CreateMap<AddBugViewModel, AddBugModel>();

            CreateMap<AddBugModel, Bug>()
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(s => DateTime.Now))
                .ForMember(d => d.LastUpdatedById, opt => opt.MapFrom(s => s.CreatorId))
                .ForMember(d => d.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(d => d.Assignee, opt => opt.Ignore())
                .ForMember(d => d.Creator, opt => opt.Ignore())
                .ForMember(d => d.Comments, opt => opt.Ignore());

            CreateMap<BugModel, Bug>()
                .ForMember(d => d.LastUpdatedBy, opt => opt.Ignore())
                .ForMember(d => d.Assignee, opt => opt.Ignore())
                .ForMember(d => d.Creator, opt => opt.Ignore())
                .ForMember(d => d.Comments, opt => opt.Ignore());

            CreateMap<BugModel, EditBugModel>();

            CreateMap<EditBugViewModel, EditBugModel>();

            CreateMap<EditBugModel, BugModel>()
                .ForMember(s => s.LastUpdatedOn, opt => opt.MapFrom(s => DateTime.Now))
                .ForMember(s => s.Description, opt => opt.Condition((s, d, m) => IsDifferentAndNotNullOrEmpty(s, d, m)))
                .ForAllMembers(opt => opt.Condition((s, d, sm) => sm != null || sm is not null));

            CreateMap<EditBugModel, Bug>();

            CreateMap<EditBugViewModel, AddBugViewModel>();

            CreateMap<Bug, BugModel>()
                .ForMember(d => d.Creator, opt => opt.MapFrom(s => s.Creator.UserName))
                .ForMember(d => d.Assignee, opt => opt.MapFrom(s => s.Assignee.Name ?? string.Empty))
                .ForMember(d => d.LastUpdatedBy, opt => opt.MapFrom(s => s.LastUpdatedBy.UserName));

            CreateMap<BugModel, BugViewModel>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Creator))
                .ForMember(d => d.AssignedTo, opt => opt.MapFrom(s => s.Assignee));
        }

        private static bool IsDifferentAndNotNullOrEmpty(EditBugModel s, BugModel d, string m)
        {
            return !String.IsNullOrWhiteSpace(m) && s.Description != d.Description;
        }
    }
}

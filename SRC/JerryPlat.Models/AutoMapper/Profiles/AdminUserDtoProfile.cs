using AutoMapper;
using JerryPlat.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Models.AutoMapper.Profiles
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<AdminUserDto, AdminUser>();
            CreateMap<AdminUser, AdminUserDto>()
                .ForMember(o => o.GroupName, o => o.MapFrom(src => src.Group.Name));

            CreateMap<GroupDto, Group>();
            CreateMap<Group, GroupDto>()
                .ForMember(o => o.NavigationIdList, o => o.MapFrom(src => src.Roles.Select(role => role.NavigationId).ToList()));

            CreateMap<Navigation, NavigationDto>();

            CreateMap<Navigation, ButtonDto>();
        }
    }
}

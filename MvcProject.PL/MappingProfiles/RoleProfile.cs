using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MvcProject.PL.ViewModels;

namespace MvcProject.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>().ForMember(D => D.RoleName , O => O.MapFrom(S=>S.Name)).ReverseMap();
        }

    }
}

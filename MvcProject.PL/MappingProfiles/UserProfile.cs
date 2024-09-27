using AutoMapper;
using MvcProject.DAL.Models;
using MvcProject.PL.ViewModels;

namespace MvcProject.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<ApplicationUser , UserViewModel>().ReverseMap();
        }
    }
}

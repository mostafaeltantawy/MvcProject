using AutoMapper;
using MvcProject.DAL.Models;
using MvcProject.PL.ViewModels;

namespace MvcProject.PL.MappingProfiles
{
    public class EmployeeProfile : Profile 
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap(); 
                //.ForMember(d => d.Name , O => O.MapFrom(S => S.EmpName)); 
        }
    }
}

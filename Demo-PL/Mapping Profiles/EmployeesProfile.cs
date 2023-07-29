using AutoMapper;
using Demo.DAL.Models;
using Demo_PL.viewModels;

namespace Demo_PL.Mapping_Profiles
{
    public class EmployeesProfile:Profile
    {
        public EmployeesProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //CreateMap<EmployeeViewModel, Employee>().ForMember(d=>d.Name , o=>o.MapFrom(s=>s.EmpName));
        }
    }
}

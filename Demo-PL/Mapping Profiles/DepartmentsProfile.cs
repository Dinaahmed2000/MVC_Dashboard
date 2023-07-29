using AutoMapper;
using Demo.DAL.Models;
using Demo_PL.viewModels;

namespace Demo_PL.Mapping_Profiles
{
    public class DepartmentsProfile:Profile
    {
        public DepartmentsProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}

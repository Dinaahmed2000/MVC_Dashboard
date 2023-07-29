using AutoMapper;
using Demo.DAL.Models;
using Demo_PL.viewModels;

namespace Demo_PL.Mapping_Profiles
{
	public class UserProfile:Profile
	{
		public UserProfile()
		{
			CreateMap<ApplicationUsers, UserViewModel>().ReverseMap();
		}
	}
}

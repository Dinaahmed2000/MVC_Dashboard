using AutoMapper;
using Demo.DAL.Models;
using Demo_PL.Helpers;
using Demo_PL.viewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo_PL.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly SignInManager<ApplicationUsers> _signInManager;
		private readonly IMapper _mapper;

		public UserController(UserManager<ApplicationUsers> userManager,SignInManager<ApplicationUsers> signInManager,IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				var users =await _userManager.Users.Select(u => new UserViewModel()
				{
					Id = u.Id,
					FName = u.FName,
					LName = u.LName,
					PhoneNumber = u.PhoneNumber,
					Email = u.Email,
					Roles = _userManager.GetRolesAsync(u).Result
				}).ToListAsync();
				return View(users);
			}
			else
			{
				var user =await _userManager.FindByEmailAsync(email);
				var mappedUser = new UserViewModel()
				{
					Id = user.Id,
					FName = user.FName,
					LName = user.LName,
					PhoneNumber = user.PhoneNumber,
					Email = user.Email,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				return View(new List<UserViewModel>(){mappedUser});
			}
		}

		//User/Details/Guid
		public async Task<IActionResult> Details(string id, string viewName = "Details")
		{
			if (id is null)
			{
				return BadRequest();
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
			{
				return NotFound();
			}

			var mappedUser = _mapper.Map<ApplicationUsers, UserViewModel>(user);

			return View(viewName, mappedUser);
		}

		//user/edit/guid
        public async Task<IActionResult> Edit(string id)
        {
         
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
					var user= await _userManager.FindByIdAsync(id);
					user.FName=userViewModel.FName;
					user.LName=userViewModel.LName;
					user.PhoneNumber=userViewModel.PhoneNumber;
					//user.Email = userViewModel.Email;
					//user.SecurityStamp=Guid.NewGuid().ToString();

                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1. Log Exception
                    //2. Friendly Message
                    //OR
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(userViewModel);
        }

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

		[ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var user=await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user); 

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error","Home");
            }
        }

    }
}

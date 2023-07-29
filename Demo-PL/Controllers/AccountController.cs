using Demo.DAL.Models;
using Demo_PL.Helpers;
using Demo_PL.viewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Demo_PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUsers> _userManager;
		private readonly SignInManager<ApplicationUsers> _signInManager;

		public AccountController(UserManager<ApplicationUsers> userManager , SignInManager<ApplicationUsers> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
        #region Register

        // /Account/Register
        public IActionResult Register ()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)  //server side validation
			{
				var user = new ApplicationUsers()
				{
					FName= model.FName,
					LName= model.LName,
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					IsAgree=model.IsAgree
				};

				var result=await _userManager.CreateAsync(user,model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}


				foreach (var error in result.Errors)
				{
                    ModelState.AddModelError(string.Empty, error.Description);
                }
			}
			return View(model);
		}
		#endregion

		#region Login
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)  //server side validation
			{ 

				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag=await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false); 
						return RedirectToAction("Index","Home");
					}
					ModelState.AddModelError(string.Empty, "Invalid Password");
				}

				ModelState.AddModelError(string.Empty, "Email is not Exist");
			}
			return View(model);
		}
		#endregion

		#region Sign out
		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user =await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token=await _userManager.GeneratePasswordResetTokenAsync(user); //token valid for this user only one time
					// https://localhost:44365/Account/ResetPassword?email=dina@gamil.com&token=24896455jhgfderfgthjk
					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new
					{
						email = user.Email,
						token=token
					}, Request.Scheme);
					var email = new Email()
					{
						Subject = "Reset Password",
						To=user.Email,
						Body= ResetPasswordLink
					};
					EmailSetting.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "Email is not Exist");
			}
			return View(model);
		}
		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion

		#region Reset Password
		public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}
		#endregion
	}
}

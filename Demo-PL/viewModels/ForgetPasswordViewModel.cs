using System.ComponentModel.DataAnnotations;

namespace Demo_PL.viewModels
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is Required ")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}

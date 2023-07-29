using System.ComponentModel.DataAnnotations;

namespace Demo_PL.viewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is Required ")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required ")]
        [Compare("NewPassword", ErrorMessage = "ConfirmPassword doesn't match Password ")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        //public string Token { get; set; }
        //public string Email { get; set; }
    }
}

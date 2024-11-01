using System.ComponentModel.DataAnnotations;

namespace GiftStore.Models.ViewModels
{
	public class RegisterViewModel
	{
        [Required(ErrorMessage = "Plrase Enter Your Email")]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "Enter you password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm your Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Enter your password correctly")]
        public string ConfirmPassword { get; set; }

        public string Mobile { get; set; }
    }
}

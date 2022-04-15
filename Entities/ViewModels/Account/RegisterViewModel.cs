using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.Account
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(30, ErrorMessage = "Maximum length for the User name is 30 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Min length is 8")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords are not the same")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

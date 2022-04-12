using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Min length is 8")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Entites.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}

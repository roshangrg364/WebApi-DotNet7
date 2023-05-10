using System.ComponentModel.DataAnnotations;

namespace Villa_Web_App.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResponseModel
    {
        public UserResponseModel User { get; set; }
        public string Token { get; set; }
    }
}

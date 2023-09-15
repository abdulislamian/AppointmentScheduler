using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set;}
    }
}

using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [MinLength(2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [MinLength(2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
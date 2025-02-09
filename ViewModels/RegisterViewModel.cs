using System.ComponentModel.DataAnnotations;

namespace TeddySmith.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name ="Email Address")]
        [Required(ErrorMessage ="Email address is required please ya3nee")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="bro password is different from the first one")]
        public string ConfirmPassword { get; set; }

    }
}

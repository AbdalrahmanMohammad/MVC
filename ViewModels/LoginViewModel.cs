using System.ComponentModel.DataAnnotations;

namespace TeddySmith.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Email Address by abd")]
        [Required(ErrorMessage ="Email address is required please ya3nee")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

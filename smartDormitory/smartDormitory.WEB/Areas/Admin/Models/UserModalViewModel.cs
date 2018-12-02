using System.ComponentModel.DataAnnotations;

namespace smartDormitory.WEB.Areas.Admin.Models
{
    public class UserModalViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }
                
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

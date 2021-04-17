using System.ComponentModel.DataAnnotations;

namespace HackVH.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Text, ErrorMessage = "{0} is text")]
        [EmailAddress]
        public string Email { get; set; }
        
        [DataType(DataType.Password, ErrorMessage = "{0} is a password")]
        [StringLength(50, MinimumLength = 7)]
        [Required]
        public string Password { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace HackVH.Models
{
    public class PasswordLoginModel
    {
        [EmailAddress(ErrorMessage = "{0} is not an email address")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} can only be 50 characters max")]
        public string Email { get; set; }
        
        [DataType(DataType.Password, ErrorMessage = "{0} is not a password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password is not between 5 and 50 characters")]
        public string Password { get; set; }
    }
}
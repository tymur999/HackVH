using System.ComponentModel.DataAnnotations;
using HackVH.Models.ViewModels;

namespace HackVH.Models
{
    public class ExternalLoginModel
    {

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Text, ErrorMessage = "{0} is text")]
        [RegularExpression(@"(^[\w-]*\.[\w-]*\.[\w-]*$)", 
            MatchTimeoutInMilliseconds = 250, ErrorMessage = "{0} is a JWT token")]
        public string Token { get; set; }
    }
}
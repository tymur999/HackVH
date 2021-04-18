using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Models
{
    public class UserModel : RegisterUserModel
    {
        public static implicit operator UserModel(IdentityUser user) => new()
        {
            Id = user.Id, 
            Email = user.Email, 
            Password = user.PasswordHash
        };

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Text, ErrorMessage = "{0} is text")]
        public string Id { get; set; }
    }
}
using System.Collections.Generic;
using HackVH.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Models.ViewModels
{
    public class UserIndexViewModel
    {
        public UserDto User { get; set; }
        
        public IEnumerable<UserLoginInfo> ExternalLogins { get; set; }
    }
}
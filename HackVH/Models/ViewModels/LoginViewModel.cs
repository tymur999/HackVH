using System;
using System.ComponentModel.DataAnnotations;

namespace HackVH.Models.ViewModels
{
    public class LoginViewModel
    {
        public ExternalLoginModel ExternalLogin { get; set; }
        public PasswordLoginModel PasswordLogin { get; set; }
    }
}
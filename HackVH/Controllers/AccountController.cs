using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.Auth;
using HackVH.Models;
using HackVH.Models.Dtos;
using HackVH.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HackVH.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return View("Error",new ErrorViewModel{RequestId = Activity.Current?.Id});

            var logins = await _userManager.GetLoginsAsync(user);
            var dto = _mapper.Map<UserDto>(user);
            
            return View(new UserIndexViewModel{User = dto, ExternalLogins = logins});
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if(user == null)
                return View("Error",new ErrorViewModel{RequestId = Activity.Current?.Id});
            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                await _signInManager.SignOutAsync();

            TempData["Result"] = JsonSerializer.Serialize(
                new ResultViewModel{Succeeded = true, Response = "Successfully deleted account"});
            return RedirectPermanent(".");
        }

        public IActionResult AccessDenied() => View();

        public async Task<IActionResult> Register()
        {
            //Sign out user
            await _signInManager.SignOutAsync();
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            var model = vm.User;
            if (!TryValidateModel(model))
            {
                return View(vm);
            }

            var user = new IdentityUser {Email = model.Email, UserName = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(vm);
            }

            TempData["Result"] = JsonSerializer.Serialize(
                new ResultViewModel{Response = "Successfully created account", Succeeded = true});
            return RedirectPermanent("./Login");
        }
        
        public async Task<IActionResult> Login()
        {
            //Sign out user
            await _signInManager.SignOutAsync();
            return View();
        }

        //Based on whether the user logs in with Google or thru password
        [HttpPost("PasswordLogin")]
        public async Task<IActionResult> PasswordLogin(LoginViewModel vm)
        {
            var model = new PasswordLoginModel{Email = vm.PasswordLogin.Email, Password = vm.PasswordLogin.Password};
            if (!TryValidateModel(model))
                return View("Login",vm);

            var user = await _userManager.FindByEmailAsync(vm.PasswordLogin.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View("Login", vm);
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.PasswordLogin.Password, 
                isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View("Login", vm);
            }
            //Show success result to the user.
            TempData["Result"] = JsonSerializer.Serialize(
                new ResultViewModel{Response = "Successfully logged in", Succeeded = true});
            
            return RedirectPermanent(".");
        }
        
        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin(LoginViewModel vm)
        {
            var model = new ExternalLoginModel{Token = vm.ExternalLogin.Token};
            if (!TryValidateModel(model))
                return View("Login", vm);

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(vm.ExternalLogin.Token);
            }
            catch(InvalidJwtException)
            {
                ModelState.AddModelError(string.Empty, "Incorrect JWT token for login");
                return View("Login");
            }

            var user = await _userManager.FindByLoginAsync("Google", payload.Subject);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    //Lets create a new user
                    user = new IdentityUser {Email = payload.Email, EmailConfirmed = true, UserName = payload.Email};
                    var result = await _userManager.CreateAsync(user);

                    if(!result.Succeeded)
                        return View("Error", new ErrorViewModel{RequestId = Activity.Current?.Id });
                }
                //Now that user is created, add the login OR
                //User exists but has not registered with Google
                await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", payload.Subject, "Google"));
            }
            
            //User already has a Google login
            await _signInManager.ExternalLoginSignInAsync("Google", payload.Subject, true);

            TempData["Result"] = JsonSerializer.Serialize(
                new ResultViewModel {Response = "Successfully logged in with Google", Succeeded = true});
            return RedirectPermanent(".");
        }
    }
}
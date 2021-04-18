using System.Threading.Tasks;
using HackVH.Models.ViewModels;
using HackVH.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HackVH.Controllers
{
    public class VideosController : Controller
    {
        private readonly IVideoService _videoService;
        private readonly UserManager<IdentityUser> _userManager;

        public VideosController(IVideoService videoService, UserManager<IdentityUser> userManager)
        {
            _videoService = videoService;
            _userManager = userManager;
        }
        //View your recommended/already watched videos
        [Authorize]
        public async Task<IActionResult> Index()
        { 
            var videos = await _videoService.GetAllVideosAsync();
            return View(new VideoSearchViewModel{Results = videos});
        }
        
        //Library will be where users can look up videos
        [Authorize]
        public IActionResult Search()
        {
            return Redirect("./Index");
        }
        
        //Search for a specific video
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Search(VideoSearchViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            
            var videos = await _videoService.SearchVideosAsync(vm.Search.SearchString);
            vm.Results = videos;
            return View(vm);
        }

        //View a specific video
        public async Task<IActionResult> Video(int id)
        {
            if (id == 0)
                return Redirect("./Index");
            var video = await _videoService.FindByIdAsync(id);
            
            if (video == null)
                return Redirect("./Index");

            var vm = new VideoViewModel {Video = video};
            var user = await _userManager.FindByIdAsync(vm.Video.User.Id);
            vm.Video.User = user;
            
            return View(vm);
        }

        [Authorize]
        public IActionResult Upload()
        {
            return View(new VideoViewModel());
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(VideoViewModel vm)
        {
            vm.Video.User = await _userManager.GetUserAsync(User);
            vm.Video.User.Password = "PlaceHolderPassword";
            
            if (!TryValidateModel(vm))
                return View(vm);
            
            await _videoService.CreateVideoAsync(vm.Video);
            return View("Video", vm);
        }
    }
}
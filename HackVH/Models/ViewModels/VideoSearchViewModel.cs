using System.Collections.Generic;
using HackVH.Data;

namespace HackVH.Models.ViewModels
{
    public class VideoSearchViewModel
    {
        public VideoSearchModel Search { get; set; }
        
        public IEnumerable<Video> Results { get; set; }
    }
}
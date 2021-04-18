using System.ComponentModel.DataAnnotations;
using HackVH.Data;

namespace HackVH.Models
{
    public class VideoModel
    {
        public static implicit operator VideoModel(Video vid) => new()
            {Id = vid.Id, User = new UserModel {Id = vid.UserId}, Name = vid.Name, VideoUrl = vid.VideoUrl};
        public int Id { get; set; }
        public UserModel User { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
        
        [Required]
        [Url]
        [RegularExpression(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$",
            MatchTimeoutInMilliseconds = 250, ErrorMessage = "{0} needs to be a Youtube video Url")]
        public string VideoUrl { get; set; }
    }
}
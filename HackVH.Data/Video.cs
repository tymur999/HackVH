using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackVH.Data
{
    public class Video
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Uploader of the video
        /// </summary>
        [Required]
        public string UserId { get; set; }
        
        
        /// <summary>
        /// Name of the video
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// The video itself. Will most likely be a youtube or vimeo link.
        /// </summary>
        [Url]
        [Required]
        public string VideoUrl { get; set; }
    }
}
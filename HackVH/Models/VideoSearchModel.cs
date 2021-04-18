using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HackVH.Models
{
    public class VideoSearchModel
    {
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Search")]
        public string SearchString { get; set; }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackVH.Data
{
    public class Unit
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public IEnumerable<Video> Videos { get; set; }
        
        [Required]
        public IEnumerable<Quiz> Quizzes { get; set; }
    }
}
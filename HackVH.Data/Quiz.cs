using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackVH.Data
{
    public class Quiz
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        
        /// <summary>
        /// Name of the quiz
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// Questions for the quiz.
        /// </summary>
        [Required]
        public IEnumerable<Question> Questions { get; set; }
    }
}
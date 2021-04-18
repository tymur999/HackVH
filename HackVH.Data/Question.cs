using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HackVH.Data
{
    public class Question
    {
        [Required]
        public string QuestionString { get; set; }
        
        [Required]
        public IEnumerable<string> Answers { get; set; }
        
        [Required]
        public int Points { get; set; }
    }
}
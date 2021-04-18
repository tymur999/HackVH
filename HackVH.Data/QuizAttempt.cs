using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Data
{
    public class QuizAttempt
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public IdentityUser User { get; set; }
        
        [Required]
        public Quiz Quiz { get; set; }
        
        /// <summary>
        /// Date at which the quiz was taken
        /// </summary>
        [Required]
        public DateTime DateOfTaking { get; set; }
        
        /// <summary>
        /// How many points the user gained.
        /// </summary>
        [Required]
        public int PointsGained { get; set; }
    }
}
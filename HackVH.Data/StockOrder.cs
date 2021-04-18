using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackVH.Data
{
    public class StockOrder
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// User id of the user who made this purchase
        /// </summary>
        [Required]
        public string UserId { get; set; }
        
        /// <summary>
        /// The individual stock price at the time of purchase
        /// </summary>
        [Required]
        public decimal StockPriceOnPurchase { get; set; }

        /// <summary>
        /// Stock code, to know which stock was purchased
        /// </summary>
        [Required]
        public string StockCode { get; set; }
        
        /// <summary>
        /// How many shares were purchased.
        /// </summary>
        [Required]
        public int Shares { get; set; }
        
        /// <summary>
        /// The time at which this order was initiated
        /// </summary>
        [Required]
        public DateTime DateOfPurchase { get; set; }
    }
}
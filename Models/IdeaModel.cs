using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models
{
    public class IdeaModel
    {

        [Key]
        public int IdeaId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal FundGoal { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public string ProductContent { get; set; }

        [ForeignKey("IdeaId")]
        public virtual List<DonationModel> Donations { get; set; }
    }
}
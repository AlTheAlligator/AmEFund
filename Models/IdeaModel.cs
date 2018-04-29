using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models
{
    public class IdeaModel
    {

        [Key]
        public int IdeaId { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }
        [DisplayName("Fund Goal")]
        [Required]
        [DataType(DataType.Currency)]
        public decimal FundGoal { get; set; }
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        [DisplayName("Product Content")]
        [Required]
        public string ProductContent { get; set; }

        [ForeignKey("IdeaId")]
        public virtual List<DonationModel> Donations { get; set; }
    }
}
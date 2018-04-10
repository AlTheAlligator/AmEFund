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
        public string ImagePath { get; set; }
        [Required]
        public string ProductContent { get; set; }

        public string ProductContentPreview { 
            get { 
                    int length = ProductContent.Length;
                    string ending = "";
                    if (length > 250)
                    {
                        length = 247;
                        ending = "...";
                    }
                    return ProductContent.Substring(0, length) + ending; 
                } 
            }

        [ForeignKey("IdeaId")]
        public virtual List<DonationModel> Donations { get; set; }
    }
}
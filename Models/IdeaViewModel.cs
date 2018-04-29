using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models
{
    public class IdeaViewModel
    {

        [Key]
        public int IdeaId { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Fund Goal")]
        [DataType(DataType.Currency)]
        public decimal FundGoal { get; set; }
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        [Required]
        [DisplayName("Product Content")]
        public string ProductContent { get; set; }

        public string ProductContentPreview { 
            get { 
                    /*int length = ProductContent.Length;
                    string ending = "";
                    if (length > 250)
                    {
                        length = 247;
                        ending = "...";
                    }
                    return ProductContent.Substring(0, length) + ending;*/
                    return "";
                } 
            }

        [ForeignKey("IdeaId")]
        public virtual List<DonationModel> Donations { get; set; }

        [Required]
        public decimal DonationAmount { get; set; }
    }
}
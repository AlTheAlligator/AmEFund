using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models
{
    public class DonationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DonationId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public int IdeaId { get; set; }
        public virtual IdeaModel Idea { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bouquet.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string RegularOption { get; set; }
        public string PremiumOption { get; set; }
        public string LuxuryOption { get; set; }

        [Required]
        [Column(TypeName = "decimal(7, 2)")]
        [Range(typeof(Decimal),"1","1000", ErrorMessage = "value must be between {1} and {2}.")]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(7, 2)")]
        [Range(1, 1000, ErrorMessage = "value must be between {1} and {2}.")]
        public decimal Price2 { get; set; }
        [Required]
        [Column(TypeName = "decimal(7, 2)")]
        [Range(1, 1000, ErrorMessage = "value must be between {1} and {2}.")]
        public decimal Price3 { get; set; }
        public string ImageUrl { get; set; }
        //
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        //
        [Required]
        public int EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public EventType EventType { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bouquet.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }   
        [Range(1,100,ErrorMessage ="Please enter a value between 1 and 100")]
        public int Count { get; set; }
        [NotMapped]
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price { get; set; }

    }
}

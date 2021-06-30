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
            Count = 0;
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

        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Count { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Count2 { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public int Count3 { get; set; }

  
        [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = "Please enter a valid Phone Number")]
        public string PhoneNumber { get; set; }
   
        public string StreetAddress { get; set; }

        public string Message { get; set; }

        public string City { get; set; }
     
        public string State { get; set; }
     
        [RegularExpression(@"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ]() ?\d[ABCEGHJKLMNPRSTVWXYZ]\d$", ErrorMessage = "Please enter a valid Postal Code")]
        public string PostalCode { get; set; }
    
        public string DeliveryName { get; set; }


        [NotMapped]
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price { get; set; }

        [NotMapped]
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price2 { get; set; }

        [NotMapped]
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price3 { get; set; }

    }
}

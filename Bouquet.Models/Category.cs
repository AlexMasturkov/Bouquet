using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bouquet.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name="Category Name")]
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bouquet.Models
{
    public class EventType
    {
        public int Id { get; set; }

        [Display(Name="Event Type")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

    }
}

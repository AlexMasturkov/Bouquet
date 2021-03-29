using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ListCarts { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}

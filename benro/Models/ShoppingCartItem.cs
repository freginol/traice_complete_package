using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{

    public partial class ShoppingCartItem
    {

        public PizzaCust pizzaCust { get; set; }
        public int ShoppingCartItemId { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}

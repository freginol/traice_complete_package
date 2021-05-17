using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int PizzaCustId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }

        public virtual Orders Order { get; set; }
        public virtual PizzaCust PizzaCust { get; set; }
    }
}

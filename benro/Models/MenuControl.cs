using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class MenuControl
    {
        public int Id { get; set; }
        public string Items { get; set; }
        public int? Discount { get; set; }
        public decimal Finalamount { get; set; }
        public string Picture { get; set; }

        public virtual Item ItemsNavigation { get; set; }
    }
}

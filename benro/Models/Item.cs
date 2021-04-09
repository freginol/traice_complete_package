using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class Item
    {
        public Item()
        {
            InvoiceInfo = new HashSet<InvoiceInfo>();
            MenuControl = new HashSet<MenuControl>();
        }

        public string Items { get; set; }
        public string Itemdescription { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<InvoiceInfo> InvoiceInfo { get; set; }
        public virtual ICollection<MenuControl> MenuControl { get; set; }
    }
}

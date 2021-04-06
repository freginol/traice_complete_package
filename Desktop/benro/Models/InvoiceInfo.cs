using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class InvoiceInfo
    {
        public InvoiceInfo()
        {
            OrderStatus = new HashSet<OrderStatus>();
        }

        public string Invoiceid { get; set; }
        public string Items { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
        public string Paymentinfo { get; set; }

        public virtual Item ItemsNavigation { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class OrderStatus
    {
        public string Ordid { get; set; }
        public int Custid { get; set; }
        public string Invoiceid { get; set; }
        public string Status { get; set; }

        public virtual Customers Cust { get; set; }
        public virtual InvoiceInfo Invoice { get; set; }
        public virtual OrderList Ord { get; set; }
    }
}

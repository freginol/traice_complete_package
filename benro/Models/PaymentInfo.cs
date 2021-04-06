using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class PaymentInfo
    {
        public int Custid { get; set; }
        public string Ordid { get; set; }
        public string Cardtype { get; set; }
        public decimal Cardvalue { get; set; }
        public decimal Cvv { get; set; }
        public DateTime Expirydate { get; set; }

        public virtual Customers Cust { get; set; }
        public virtual OrderList Ord { get; set; }
    }
}

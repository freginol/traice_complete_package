using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class PaymentInfo
    {
        public string CustomerName { get; set; }
        public string CardType { get; set; }
        public decimal CardValue { get; set; }
        public decimal Cvv { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}

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
        public string CardValue { get; set; }
        public decimal CVV { get; set; }
        public string ExpiryDate { get; set; }
    }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class Customers
    {
        public Customers()
        {
            OrderList = new HashSet<OrderList>();
            OrderStatus = new HashSet<OrderStatus>();
            PaymentInfo = new HashSet<PaymentInfo>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Addrses1 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Postal { get; set; }
        public string Phone { get; set; }
        public string Emailaddress { get; set; }

        public virtual CustFeedback CustFeedback { get; set; }
        public virtual ICollection<OrderList> OrderList { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
        public virtual ICollection<PaymentInfo> PaymentInfo { get; set; }
    }
}

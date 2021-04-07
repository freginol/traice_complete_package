using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class OrderList
    {
        public OrderList()
        {
            OrderStatus = new HashSet<OrderStatus>();
        }

        public string Ordid { get; set; }
        public string Custname { get; set; }
        public string Address { get; set; }
        public string Ordertype { get; set; }
        public string Discountcode { get; set; }
        public decimal Totalamount { get; set; }
        public int? Custid { get; set; }

        public virtual Customers Cust { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}

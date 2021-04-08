using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class Orders
    {
        public List<OrderDetail> OrderLines { get; set; }

        public string FirstName { get; set; }
        public int OrderId { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string OrderPlaced { get; set; }
        public decimal OrderTotal { get; set; }

        //public virtual ICollection<OrderDetail> OrderLines { get; set; }
    }
}

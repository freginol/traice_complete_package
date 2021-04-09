using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
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
        }

        public int Id { get; set; }
        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression("^[a-zA-Z_ ]{2,20}$", ErrorMessage = "Firstname is not Valid")]
        public string FirstName { get; set; }
        [Display(Name = "Lastname")]
        [Required(ErrorMessage = "Lastname is required")]
        [RegularExpression("^[a-zA-Z_ ]{2,20}$", ErrorMessage = "Lastname is not Valid")]
        public string LastName { get; set; }
        [Display(Name = "Address")]
        public string Addrses1 { get; set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        [RegularExpression("^[a-zA-Z_ ]{2,20}$", ErrorMessage = "City is not Valid")]
        public string City { get; set; }
        [Display(Name = "Province")]
        [Required(ErrorMessage = "Province is required")]
        [RegularExpression("^[a-zA-Z_ ]{2,20}$", ErrorMessage = "Province is not Valid")]
        public string Province { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Display(Name = "Postal")]
        [Required(ErrorMessage = "Postal is required")]
        public string Postal { get; set; }
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Emailaddress { get; set; }


        public virtual ICollection<OrderList> OrderList { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}
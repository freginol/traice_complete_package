using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class CustFeedback
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Feedback { get; set; }

    }
}

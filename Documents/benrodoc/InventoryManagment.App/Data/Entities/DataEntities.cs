using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagment.App.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() => InventoryItems = new HashSet<InventoryItems>();

        public new string Id { get; set; }

        public new string Email { get; set; }

        public new string UserName { get; set; }

        public virtual ICollection<InventoryItems> InventoryItems { get; set; }
    }

    public class InventoryItems
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public double ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public bool IsSoldOut { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
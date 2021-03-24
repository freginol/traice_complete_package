using System;
using System.ComponentModel;

namespace InventoryManagement.App.Models
{
    public class InventoryItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Item Type")]
        public string ItemType { get; set; }
        [DisplayName("Item Description")]
        public string ItemDescription { get; set; }
        [DisplayName("Price")]
        public double ItemPrice { get; set; }
        [DisplayName("Quantity")]
        public int ItemQuantity { get; set; }
        [DisplayName("Sold Out")]
        public bool IsSoldOut { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
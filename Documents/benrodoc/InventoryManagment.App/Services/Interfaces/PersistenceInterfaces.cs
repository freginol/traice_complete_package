using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagement.App.Models;

namespace InventoryManagment.App.Services.Interfaces
{
    public interface IStoreManagementRepository
    {
        Task<IEnumerable<InventoryItemViewModel>> GetInventoryItems();

        Task<IEnumerable<InventoryItemViewModel>> GetInventoryItemsByUser(string userId);

        Task<InventoryItemViewModel> GetInventoryItem(string inventoryId);

        Task UpdateInventoryItem(string inventoryId, InventoryItemViewModel model);

        Task DeleteInventoryItem(string inventoryId);

        Task CreateInventoryItem(InventoryItemViewModel model);
    }
}

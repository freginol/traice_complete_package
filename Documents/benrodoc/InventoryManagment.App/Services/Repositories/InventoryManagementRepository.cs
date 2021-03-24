using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.App.Models;
using InventoryManagment.App.Data;
using InventoryManagment.App.Data.Entities;
using InventoryManagment.App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace InventoryManagement.App.Services.Repositories
{
    public class InventoryManagementRepository : IStoreManagementRepository
    {
        public readonly ApplicationDbContext _dbContext;
        public readonly ILogger<InventoryManagementRepository> _logger;

        public InventoryManagementRepository(ApplicationDbContext dbContext, ILogger<InventoryManagementRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task CreateInventoryItem(InventoryItemViewModel model)
        {
            var inventoryRecord = new InventoryItems
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                ItemDescription = model.ItemDescription,
                CreatedBy = model.CreatedBy,
                ModifiedOn = DateTime.Now,
                IsSoldOut = model.IsSoldOut,
                ItemType = model.ItemType,
                ItemPrice = model.ItemPrice,
                ItemQuantity = model.ItemQuantity
            };

            _dbContext.InventoryItems.Add(inventoryRecord);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteInventoryItem(string inventoryId)
        {
            var inventoryRecord = await _dbContext.InventoryItems.FindAsync(inventoryId);

            if (inventoryRecord == null) throw new Exception("Database record doesn't exist!");

            _dbContext.InventoryItems.Remove(inventoryRecord);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<InventoryItemViewModel> GetInventoryItem(string inventoryId)
        {
            return await _dbContext.InventoryItems.Select(q => new InventoryItemViewModel
            {
                Id = q.Id,
                Name = q.Name,
                ItemDescription = q.ItemDescription,
                CreatedBy = q.CreatedBy,
                ModifiedOn = q.ModifiedOn,
                IsSoldOut = q.IsSoldOut,
                ItemType = q.ItemType,
                ItemPrice = q.ItemPrice,
                ItemQuantity = q.ItemQuantity
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<InventoryItemViewModel>> GetInventoryItems()
        {
            return await _dbContext.InventoryItems.Select(q => new InventoryItemViewModel
            {
                Id = q.Id,
                Name = q.Name,
                ItemDescription = q.ItemDescription,
                CreatedBy = q.CreatedBy,
                ModifiedOn = q.ModifiedOn,
                IsSoldOut = q.IsSoldOut,
                ItemType = q.ItemType,
                ItemPrice = q.ItemPrice,
                ItemQuantity = q.ItemQuantity
            }).ToListAsync();
        }

        public async Task<IEnumerable<InventoryItemViewModel>> GetInventoryItemsByUser(string userId)
        {
            return await _dbContext.InventoryItems.Where(s => s.CreatedBy == userId).Select(q => new InventoryItemViewModel
            {
                Id = q.Id,
                Name = q.Name,
                ItemDescription = q.ItemDescription,
                CreatedBy = q.CreatedBy,
                ModifiedOn = q.ModifiedOn,
                IsSoldOut = q.IsSoldOut,
                ItemType = q.ItemType,
                ItemPrice = q.ItemPrice,
                ItemQuantity = q.ItemQuantity
            }).ToListAsync();
        }

        public async Task UpdateInventoryItem(string inventoryId, InventoryItemViewModel model)
        {
            var inventoryRecord = await _dbContext.InventoryItems.FindAsync(inventoryId);

            if (inventoryRecord == null) throw new Exception("Database record doesn't exist!");

            inventoryRecord.Id = model.Id;
            inventoryRecord.Name = model.Name;
            inventoryRecord.ItemDescription = model.ItemDescription;
            inventoryRecord.CreatedBy = model.CreatedBy;
            inventoryRecord.ModifiedOn = DateTime.Now;
            inventoryRecord.IsSoldOut = model.IsSoldOut;
            inventoryRecord.ItemType = model.ItemType;
            inventoryRecord.ItemPrice = model.ItemPrice;
            inventoryRecord.ItemQuantity = model.ItemQuantity;

            _dbContext.InventoryItems.Update(inventoryRecord);

            await _dbContext.SaveChangesAsync();
        }
    }
}
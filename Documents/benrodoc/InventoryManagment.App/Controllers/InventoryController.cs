using System.Security.Claims;
using System.Threading.Tasks;
using InventoryManagement.App.Models;
using InventoryManagment.App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.App.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IStoreManagementRepository _inventoryRepo;

        public InventoryController(ILogger<InventoryController> logger, IStoreManagementRepository storeManagementRepository)
        {
            _logger = logger;
            _inventoryRepo = storeManagementRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            
            var inventoryItems = await _inventoryRepo.GetInventoryItemsByUser(currentUserId);

            return View(inventoryItems);
        }

        public IActionResult Create()
        {
            var inventoryItem = new InventoryItemViewModel {};

            return View(inventoryItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _inventoryRepo.CreateInventoryItem(model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            InventoryItemViewModel inventoryItem = await _inventoryRepo.GetInventoryItem(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            InventoryItemViewModel inventoryItem = await _inventoryRepo.GetInventoryItem(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, InventoryItemViewModel model)
        {
            if (ModelState.IsValid)
            {

                await _inventoryRepo.UpdateInventoryItem(id, model);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            InventoryItemViewModel inventoryItem = await _inventoryRepo.GetInventoryItem(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _inventoryRepo.DeleteInventoryItem(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaKnight.Models;

namespace PizzaKnight.Controllers
{
    public class PizzaCustController : Controller
    {
        private readonly _5510Context _context;

        public PizzaCustController(_5510Context context)
        {
            _context = context;
        }

        // GET: PizzaCust
        public async Task<IActionResult> Index()
        {
            return View(await _context.PizzaCust.ToListAsync());
        }

        // GET: PizzaCust/Details/5
        public async Task<PizzaCust> Details(int? id)
        {
            if (id == null)
            {
               // return NotFound();
            }

            var pizzaCust = await _context.PizzaCust
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaCust == null)
            {
                //return NotFound();
            }

            return pizzaCust;
        }

        // GET: PizzaCust/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PizzaCust/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,CategoriesId,ImageUrl")] PizzaCust pizzaCust)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pizzaCust);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pizzaCust);
        }

        // GET: PizzaCust/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaCust = await _context.PizzaCust.FindAsync(id);
            if (pizzaCust == null)
            {
                return NotFound();
            }
            return View(pizzaCust);
        }

        // POST: PizzaCust/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,CategoriesId,ImageUrl")] PizzaCust pizzaCust)
        {
            if (id != pizzaCust.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzaCust);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaCustExists(pizzaCust.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pizzaCust);
        }

        // GET: PizzaCust/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaCust = await _context.PizzaCust
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaCust == null)
            {
                return NotFound();
            }

            return View(pizzaCust);
        }

        // POST: PizzaCust/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzaCust = await _context.PizzaCust.FindAsync(id);
            _context.PizzaCust.Remove(pizzaCust);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaCustExists(int id)
        {
            return _context.PizzaCust.Any(e => e.Id == id);
        }
    }
}

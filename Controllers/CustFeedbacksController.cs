using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PizzaKnight.Controllers
{
    public class CustFeedbacksController : Controller
    {
        private readonly Models._5510Context _context;

        public CustFeedbacksController(Models._5510Context context)
        {
            _context = context;
        }

        // GET: CustFeedbacks
        public async Task<IActionResult> Index()
        {
            //var _5510Context = _context.CustFeedback.Include(c => c.FirstName);
            return View(await _context.CustFeedback.ToListAsync());
        }

        // GET: CustFeedbacks/Details/5
        public async Task<IActionResult> Details(string FirstName)
        {
            if (FirstName == null)
            {
                return NotFound();
            }

            var custFeedback = await _context.CustFeedback
                .Include(c => c.FirstName)
                .FirstOrDefaultAsync(m => m.FirstName.Equals(FirstName));
            if (custFeedback == null)
            {
                return NotFound();
            }

            return View(custFeedback);
        }

        // GET: CustFeedbacks/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: CustFeedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Feedback")] Models.CustFeedback custFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(custFeedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
  
            return View(custFeedback);
        }

        // GET: CustFeedbacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var custFeedback = await _context.CustFeedback.FindAsync(id);
            if (custFeedback == null)
            {
                return NotFound();
            }
            
            return View(custFeedback);
        }

        
        // GET: CustFeedbacks/Delete/5
        public async Task<IActionResult> Delete(string FirstName)
        {
            if (FirstName == null)
            {
                return NotFound();
            }

            var custFeedback = await _context.CustFeedback
                .Include(c => c.FirstName)
                .FirstOrDefaultAsync(m => m.FirstName.Equals(FirstName));
            if (custFeedback == null)
            {
                return NotFound();
            }

            return View(custFeedback);
        }

        // POST: CustFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string name)
        {
            var custFeedback = await _context.CustFeedback.FindAsync(name);
            _context.CustFeedback.Remove(custFeedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

 
    }
}

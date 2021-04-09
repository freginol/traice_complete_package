using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PizzaKnight.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Models._5510Context _context;

        public CustomersController(Models._5510Context context)
        {
            _context = context;
        }
        
        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }


        // GET: Customers/Create
        
        public async Task<IActionResult> Create()
        {
          
            return View("Create");
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Addrses1,City,Province,Country,Postal,Phone,Emailaddress")] Models.Customers customers)
        {
            if (ModelState.IsValid)
            {
                string canada = "^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$";
                string USA = "^[0-9]{5}(?:-[0-9]{4})?$";
                Console.WriteLine("(((((((((((((((" + customers.Country);
                Console.WriteLine("**********" + canada);
                //customers.Id = customers.Id + 1;
                if (string.Equals(customers.Country, "Canada"))
                {

                    if (System.Text.RegularExpressions.Regex.IsMatch(customers.Postal, canada))
                    {
                        _context.Add(customers);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", "PaymentInfoes");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Canadian Postal Code");
                        return View(customers);
                    }
                }
                else if (string.Equals(customers.Country, "USA"))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(customers.Postal, USA))
                    {
                        _context.Add(customers);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Create", "PaymentInfoes");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid American Postal Code");
                        return View(customers);
                    }
                }
            else
            {
                ModelState.AddModelError("", "Invalid details");
                return View(customers);
            }
          

                
                
            }
            return View(customers);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            return View(customers);
        }
        
        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Addrses1,City,Province,Country,Postal,Phone,Emailaddress")] Models.Customers customers)
        {
            if (id != customers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.Id))
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
            return View(customers);
        }
        
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customers = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}

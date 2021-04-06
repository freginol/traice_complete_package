using Microsoft.EntityFrameworkCore;
using PizzaKnight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaKnight.Repositories
{

    public class PizzaRepository : IPizzaRepository
    {
        private readonly _5510Context _context;
       

        public PizzaRepository(_5510Context context)
        {
            _context = context;
        }


        public void Add(PizzaCust pizza)
        {
            _context.Add(pizza);
        }

        public IEnumerable<PizzaCust> GetAll()
        {
            return _context.PizzaCust.ToList();
        }

        public async Task<IEnumerable<PizzaCust>> GetAllAsync()
        {
            return await _context.PizzaCust.ToListAsync();
        }



        public PizzaCust GetById(int? id)
        {
            return _context.PizzaCust.FirstOrDefault(p => p.Id == id);
        }

        public async Task<PizzaCust> GetByIdAsync(int? id)
        {
            return await _context.PizzaCust.FirstOrDefaultAsync(p => p.Id == id);
        }





        public bool Exists(int id)
        {
            return _context.PizzaCust.Any(p => p.Id == id);
        }

        public void Remove(PizzaCust pizza)
        {
            _context.Remove(pizza);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(PizzaCust pizza)
        {
            _context.Update(pizza);
        }

    }
}

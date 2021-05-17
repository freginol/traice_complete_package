using PizzaKnight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaKnight.Repositories
{
    public interface IPizzaRepository
    {
    

        PizzaCust GetById(int? id);
        Task<PizzaCust> GetByIdAsync(int? id);

       
    

        bool Exists(int id);

        IEnumerable<PizzaCust> GetAll();
        Task<IEnumerable<PizzaCust>> GetAllAsync();



        void Add(PizzaCust pizza);
        void Update(PizzaCust pizza);
        void Remove(PizzaCust pizza);

        void SaveChanges();
        Task SaveChangesAsync();

    }
}

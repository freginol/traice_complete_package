using PizzaKnight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaKnight.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Orders order);

    }
}


using PizzaKnight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaKnight.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly _5510Context _context;
        private readonly ShoppingCart _shoppingCart;


        public OrderRepository(_5510Context context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }
        
        public async Task CreateOrderAsync(Orders order)
        {
            DateTime today = DateTime.Today;
            //order.OrderPlaced = today;
            decimal totalPrice = 0M;

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    PizzaCustId = shoppingCartItem.pizzaCust.Id,
                    Order = order,
                    Price = shoppingCartItem.pizzaCust.Price,
                    
                };
                totalPrice += orderDetail.Price * orderDetail.Amount;
                _context.OrderDetail.Add(orderDetail);
            }

            order.OrderTotal = totalPrice;
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public  class ShoppingCart
    {
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        private readonly _5510Context _context;
  
        public ShoppingCart(_5510Context context)
        {
            _context = context;
        }
        public async Task AddToCartAsync(PizzaCust pizzaCust, int amount)
        {
            var shoppingCartItem =
            await _context.ShoppingCartItem.SingleOrDefaultAsync(
            s => s.pizzaCust.Id == pizzaCust.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    pizzaCust = pizzaCust,
                    Amount = 1
                };

                _context.ShoppingCartItem.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            await _context.SaveChangesAsync();
        }
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
            .HttpContext.Session;

            var context = services.GetService<_5510Context>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
            return ShoppingCartItems ??
            (ShoppingCartItems = await
            _context.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
            .Include(s => s.pizzaCust)
            .ToListAsync());
        }
        public decimal GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
            .Select(c => c.pizzaCust.Price * c.Amount).Sum();
            return total;
        }

    }

}

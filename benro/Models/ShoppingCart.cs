using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaKnight.Models
{
    public class ShoppingCart
    {
        private readonly _5510Context _appDbContext;

        private ShoppingCart(_5510Context appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<_5510Context>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task AddToCartAsync(PizzaCust pizza, int amount)
        {
            Console.WriteLine(" ia m in ShoppingCart");
            var shoppingCartItem =
                    await _appDbContext.ShoppingCartItem.SingleOrDefaultAsync(
                        s => s.pizzaCust.Id == pizza.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    pizzaCust = pizza,
                    Amount = 1
                };

                Console.WriteLine("hhhhhhhhhhhhhhhhhhhh" + shoppingCartItem.ShoppingCartId);
                //Console.WriteLine("hhhhhhhhhjjjjjjjjjjj" + shoppingCartItem.ShoppingCartItemId);
                Console.WriteLine("nnnnnnnnnnn" + shoppingCartItem.PizzaCustId);
                Console.WriteLine(" vvvvvvvvsssssssss" + shoppingCartItem.Amount);
                
                _appDbContext.ShoppingCartItem.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveFromCartAsync(PizzaCust pizza)
        {
            var shoppingCartItem =
                    await _appDbContext.ShoppingCartItem.SingleOrDefaultAsync(
                        s => s.pizzaCust.Id == pizza.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShoppingCartItem.Remove(shoppingCartItem);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return localAmount;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = await
                       _appDbContext.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.pizzaCust)
                           .ToListAsync());
        }

        public async Task ClearCartAsync()
        {
            var cartItems = _appDbContext
                .ShoppingCartItem
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItem.RemoveRange(cartItems);

            await _appDbContext.SaveChangesAsync();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.pizzaCust.Price * c.Amount).Sum();
            return total;
        }

    }
}

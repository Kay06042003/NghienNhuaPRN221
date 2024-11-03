using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAO
{
    public class CartDAO : SingletonBase<CartDAO>
    {
        public async Task<IEnumerable<Cart>> GetCartByUserId(int userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Cart> GetCartById(int cartId)
        {
            return await _context.Carts
                .Include(c => c.Pro)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        // get cart by user id and product id
        public async Task<Cart> GetCartByUserIdAndProductId(int userId, int productId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProId == productId);
        }

        // update cart
        public async Task UpdateCart(int cartId, int quantity)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
            cart.CartQuantity = quantity;
            await _context.SaveChangesAsync();
        }

        // remove cart
        public async Task RemoveCart(int cartId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        // add cart
        public async Task AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartRange(IEnumerable<Cart> carts)
        {
            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();
        }
    }
}

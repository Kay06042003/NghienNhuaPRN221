using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BusinessLogic.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartByUserId(int userId);
        Task<Cart> GetCartById(int cartId);
        Task UpdateCart(int cartId, int quantity);
        Task RemoveCart(int cartId);
        Task<Cart> GetCartByUserIdAndProductId(int userId, int productId);
        Task AddCart(Cart cart);
        Task RemoveCartRange(IEnumerable<Cart> carts);
    }
}
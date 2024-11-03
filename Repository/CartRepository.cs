using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DAO;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDAO _cartDAO;
        public CartRepository(CartDAO cartDAO)
        {
            _cartDAO = cartDAO;
        }

        public async Task AddCart(Cart cart)
        {
            await _cartDAO.AddCart(cart);
        }

        public async Task<Cart> GetCartById(int cartId)
        {
            return await _cartDAO.GetCartById(cartId);
        }

        public async Task<IEnumerable<Cart>> GetCartByUserId(int userId)
        {
            return await _cartDAO.GetCartByUserId(userId);
        }

        public async Task<Cart> GetCartByUserIdAndProductId(int userId, int productId)
        {
            return await _cartDAO.GetCartByUserIdAndProductId(userId, productId);
        }

        public async Task RemoveCart(int cartId)
        {
            await _cartDAO.RemoveCart(cartId);
        }

        public async Task RemoveCartRange(IEnumerable<Cart> carts)
        {
            await _cartDAO.RemoveCartRange(carts);
        }

        public async Task UpdateCart(int cartId, int quantity)
        {
            await _cartDAO.UpdateCart(cartId, quantity);
        }

        




    }
}
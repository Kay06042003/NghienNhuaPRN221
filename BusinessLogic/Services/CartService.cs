using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Models;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetCartById(int cartId)
        {
            return await _cartRepository.GetCartById(cartId);
        }

        public async Task<IEnumerable<Cart>> GetCartByUserId(int userId)
        {
            return await _cartRepository.GetCartByUserId(userId);
        }

        public async Task RemoveCart(int cartId)
        {
            await _cartRepository.RemoveCart(cartId);
        }

        public async Task UpdateCart(int cartId, int quantity)
        {
            await _cartRepository.UpdateCart(cartId, quantity);
        }
        public async Task<Cart> GetCartByUserIdAndProductId(int userId, int productId)
        {
            return await _cartRepository.GetCartByUserIdAndProductId(userId, productId);
        }

        public async Task AddCart(Cart cart)
        {
            await _cartRepository.AddCart(cart);
        }

        public async Task RemoveCartRange(IEnumerable<Cart> carts)
        {
            await _cartRepository.RemoveCartRange(carts);
        }
    }
}
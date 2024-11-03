using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using NghienNhuaMVC.Middleware;

namespace NghienNhuaMVC.Controllers
{

    [TypeFilter(typeof(UserAuthorizationFilter))]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserServices _userServices;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IUserServices userServices, IProductService productService)
        {
            _cartService = cartService;
            _userServices = userServices;
            _productService = productService;
        }


        public async Task<IActionResult> Index()
        {
            var acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("AccGmail"));
            User user = await _userServices.getUserAsync(acc.AccId);
            var cart = await _cartService.GetCartByUserId(user.UserId);
            if (cart.Count() == 0)
            {
                TempData["Message"] = "ErrorCart";
                return RedirectToAction("Index", "Home");
            }
            var allProducts = await _productService.GetAll();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in cart)
            {
                pro = allProducts.FirstOrDefault(p => p.ProId == item.ProId);
                products.Add(pro);
            }
            // Send product to view
            ViewBag.products = products;
            return View(cart);
        }
        public async Task<IActionResult> UpdateCart(int cartId, int productId, int quantity)
        {
            // get product from cart
            Cart cart = await _cartService.GetCartById(cartId);

            // update quantity
            Product product = await _productService.GetProductByProID(productId);
            if (quantity > product.ProQuantity)
            {
                return Json(new { status = "error" });
            }
            await _cartService.UpdateCart(cart.CartId, quantity);

            int money = int.Parse(product.ProPrice);
            int quantityProductInCart = int.Parse(cart.CartQuantity.ToString());
            int total = money * quantityProductInCart;
            int totalMoney = 0;
            IEnumerable<Cart> carts = await _cartService.GetCartByUserId(cart.UserId ?? 0);
            IEnumerable<Product> allProducts = await _productService.GetAll();

            foreach (var item in carts)
            {
                totalMoney += int.Parse(allProducts.FirstOrDefault(p => p.ProId == item.ProId).ProPrice) * int.Parse(item.CartQuantity.ToString());
            }
            int final = totalMoney + 20000;
            // convert money and totalMoeny before return
            // Nghien_Nhua.MyUtil.ConvertFunction convert = new Nghien_Nhua.MyUtil.ConvertFunction();

            string totalMoneyString = totalMoney.ToString("N0");
            string totalString = total.ToString("N0");
            string finalString = final.ToString("N0");

            return Json(new { totalString, totalMoneyString, finalString });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0 || id == null)
            {
                TempData["Message"] = "error";
                return RedirectToAction("Index");
            }
            try
            {
                await _cartService.RemoveCart(id);
                var acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("AccGmail"));
                User user2 = await _userServices.getUserAsync(acc.AccId);
                var cart2 = await _cartService.GetCartByUserId(user2.UserId);
                if (cart2.Count() == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                TempData["Message"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["Message"] = "error";
                return RedirectToAction("Index");
            }
        }

    }
}
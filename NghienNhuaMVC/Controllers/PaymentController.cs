using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using NghienNhuaMVC.Middleware;
using NghienNhuaMVC.Services;

namespace NghienNhuaMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayServices _vnPayServices;
        private readonly IOrderServices _orderServices;
        private readonly IAccountServices _accountServices;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public PaymentController(
            IVnPayServices vnPayServices, 
            IOrderServices orderServices, 
            IAccountServices accountServices, 
            ICartService cartService,
            IProductService productService
            )

        {
            _vnPayServices = vnPayServices;
            _orderServices = orderServices;
            _accountServices = accountServices;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Return()
        {
            var response = _vnPayServices.PaymentExcute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction("Index", "Error");
            }
            string email = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(email);
            Account user = await _accountServices.getUserAsync(account.AccGmail);

            string orderJson = HttpContext.Session.GetString("Order");
            Order order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(orderJson);
            await _orderServices.addOrder(order);

            IEnumerable<Cart> carts = await _cartService.GetCartByUserId(user.User.UserId);
            IEnumerable<Product> AllProduct = await _productService.GetAll();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = AllProduct.FirstOrDefault(p => p.ProId == item.ProId);
                products.Add(pro);
            }
            
            foreach (var item in carts)
            {
                OrderDetail oDetails = new OrderDetail();
                oDetails.OrderId = order.OrderId;
                oDetails.ProId = item.ProId;
                oDetails.OdPrice = products.FirstOrDefault(p => p.ProId == item.ProId).ProPrice;
                oDetails.OdQuantity = item.CartQuantity;
                oDetails.OdTotalMoney = (item.CartQuantity * int.Parse(products.FirstOrDefault(p => p.ProId == item.ProId).ProPrice)).ToString();
                await _orderServices.AddOrderDetail(oDetails);
            }
            await _cartService.RemoveCartRange(carts);
            return View();
        }

    }
}
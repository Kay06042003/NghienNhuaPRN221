using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Models;
using Newtonsoft.Json;
using NghienNhuaMVC.Models;
using NghienNhuaMVC.Services;

namespace NghienNhuaMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IAccountServices _accountServices;
        private readonly IProductService _productServices;
        private readonly ICartService _cartService;
        private readonly IVnPayServices _vnPayServices;
        public OrderController(IOrderServices orderServices, IAccountServices accountServices, IProductService productServices, ICartService cartService, IVnPayServices vnPayServices)
        {
            _orderServices = orderServices;
            _accountServices = accountServices;
            _productServices = productServices;
            _cartService = cartService;
            _vnPayServices = vnPayServices;
        }

        public async Task<IActionResult> Order()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                TempData["Message"] = "login";
                return RedirectToAction("Index", "Home");
            }
            Account account = JsonConvert.DeserializeObject<Account>(email);
            Account accUser = await _accountServices.getUserAsync(account.AccGmail);
            IEnumerable<Cart> carts = await _cartService.GetCartByUserId(accUser.User.UserId);
            IEnumerable<Product> AllProduct = await _productServices.GetAll();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = AllProduct.FirstOrDefault(p => p.ProId == item.ProId);
                if (pro.ProQuantity < item.CartQuantity)
                {
                    TempData["Message"] = "ErrorOrder";
                    return RedirectToAction("Index", "Cart");
                }
                products.Add(pro);
            }
            ViewBag.products = products;
            return View(carts);
        }

        // action history order
        public async Task<IActionResult> History()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(email);
            Account accUser = await _accountServices.getUserAsync(account.AccGmail);

            IEnumerable<Order> orders = await _orderServices.getOrders(accUser.User.UserId);
            if (orders.Count() == 0)
            {
                return RedirectToAction("CreateError", "Error", new { message = "Bạn chưa có đơn đặt hàng nào trước đó!" });
            }
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in orders)
            {
                IEnumerable<OrderDetail> details = await _orderServices.GetOrderDetails(item.OrderId);
                foreach (var detail in details)
                {
                    orderDetails.Add(detail);
                }
            }
            List<Product> products = new List<Product>();
            foreach (var item in orderDetails)
            {
                Product pro = await _productServices.GetProductByProID(item.ProId ?? 0);
                products.Add(pro);
            }

            ViewBag.orderDetails = orderDetails;
            ViewBag.products = products;
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(IFormCollection form)
        {
            string name = form["txtName"];
            string phone = form["txtPhone"];
            string emailFrom = form["txtEmail"];
            string address = form["txtAddress"];
            string payment = form["txtpayment"];
            double total = Double.Parse(form["amount"]);
            string email = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(email);
            Account user = await _accountServices.getUserAsync(account.AccGmail);
            Order order = new Order();
            order.UserId = user.User.UserId;
            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Waiting Accept - COD";
            order.OrderName = name;
            order.OrderPhoneNumber = phone;
            order.OrderEmail = emailFrom;
            order.OrderAddress = address;
            order.OrderTotalMoney = total.ToString();
            // convert order to json
            IEnumerable<Cart> carts = await _cartService.GetCartByUserId(user.User.UserId);
            IEnumerable<Product> AllProduct = await _productServices.GetAll();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = AllProduct.FirstOrDefault(p => p.ProId == item.ProId);
                if (pro.ProQuantity < item.CartQuantity)
                {
                    item.CartQuantity = pro.ProQuantity;
                    await _cartService.UpdateCart(item.CartId, pro.ProQuantity??0);
                    TempData["Message"] = "ErrorOrder";
                    return RedirectToAction("Index", "Cart");
                }
                products.Add(pro);
            }

            switch (payment)
            {
                case "QuetMaQR":
                case "TKNganHang":
                case "QuocTe":
                    VnPayResqestModel vnPayment = new VnPayResqestModel
                    {
                        amount = total,
                        CreatedDate = DateTime.Now,
                        OrderId = 1,
                        fullName = name,
                        phone = phone,
                        email = emailFrom,
                        address = address
                    };
                    order.OrderStatus = "Accept - Banking";
                    string orderJson = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                    HttpContext.Session.SetString("Order", orderJson);
                    return Redirect(_vnPayServices.CreateRequestUrl(HttpContext, vnPayment));
                case "NhanHang":
                    try
                    {
                        await _orderServices.addOrder(order);
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Index", "Cart");
                    }

                    // get order id from order table with user id and last order date
                    // int orderID = _db.Orders.Where(p => p.UserId == user.UserId).OrderByDescending(p => p.OrderId).FirstOrDefault().OrderId;

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
                    break;
            }

            return RedirectToAction("Thanks", "Error");
        }
    }
}
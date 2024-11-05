using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLogic.ViewModels;
using NghienNhuaMVC.Middleware;
using NghienNhuaMVC.Models;
using Newtonsoft.Json;

namespace NghienNhuaMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAccountServices _account;
        private readonly ICartService _cartService;

        public ProductController(IProductService productService, IAccountServices account, ICartService cartService)
        {
            _productService = productService;
            _account = account;
            _cartService = cartService;
        }


        public async Task<IActionResult> Keyboard()
        {
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.Where(p => p.ProCategory == "Keyboard");
            var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
            KeyboardListViewModel model = new KeyboardListViewModel()
            {
                Products = products,
                newProducts = newProducts
            };
            return View(model);
        }

        public async Task<IActionResult> Mouse()
        {
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.Where(p => p.ProCategory == "Mouse");
            var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
            KeyboardListViewModel model = new KeyboardListViewModel()
            {
                Products = products,
                newProducts = newProducts
            };
            return View("Keyboard", model);
        }

        public async Task<IActionResult> Kit()
        {
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.Where(p => p.ProCategory == "Kit");
            var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
            KeyboardListViewModel model = new KeyboardListViewModel()
            {
                Products = products,
                newProducts = newProducts
            };

            return View("Keyboard", model);
        }

        public async Task<IActionResult> Keycap()
        {
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.Where(p => p.ProCategory == "Keycap");
            var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
            KeyboardListViewModel model = new KeyboardListViewModel()
            {
                Products = products,
                newProducts = newProducts
            };
            return View("Keyboard", model);
        }

        public async Task<IActionResult> Switch()
        {
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.Where(p => p.ProCategory == "Switch");
            var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
            KeyboardListViewModel model = new KeyboardListViewModel()
            {
                Products = products,
                newProducts = newProducts
            };
            return View("Keyboard", model);
        }

        public async Task<IActionResult> Earphone()
        {
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.Where(p => p.ProCategory == "Earphone");
            var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
            KeyboardListViewModel model = new KeyboardListViewModel()
            {
                Products = products,
                newProducts = newProducts
            };
            return View("Keyboard", model);
        }

        public async Task<IActionResult> ProductDetail(int id, string proCategory)
        {
            IEnumerable<Product> products = await _productService.GetAll();
            switch (proCategory)
            {
                case "Keyboard":
                    var product = products.Where(p => p.ProCategory == "Keyboard" && p.ProId == id).FirstOrDefault();
                    if (product == null)
                    {
                        return RedirectToAction("Keyboard");
                    }
                    var newProducts = products.OrderByDescending(p => p.ProId).Take(2).ToList();
                    ProductDetailViewModel productDetailViewModel = new ProductDetailViewModel()
                    {
                        Product = product,
                        newProducts = newProducts
                    };
                    return View(productDetailViewModel);
                case "Mouse":
                    var product1 = products.Where(p => p.ProCategory == "Mouse" && p.ProId == id).FirstOrDefault();
                    if (product1 == null)
                    {
                        return RedirectToAction("Mouse");
                    }
                    var newProducts1 = products.OrderByDescending(p => p.ProId).Take(2).ToList();
                    ProductDetailViewModel productDetailViewModel1 = new ProductDetailViewModel()
                    {
                        Product = product1,
                        newProducts = newProducts1
                    };
                    return View(productDetailViewModel1);
                case "Kit":
                    var product2 = products.Where(p => p.ProCategory == "Kit" && p.ProId == id).FirstOrDefault();
                    if (product2 == null)
                    {
                        return RedirectToAction("Kit");
                    }
                    var newProducts2 = products.OrderByDescending(p => p.ProId).Take(2).ToList();
                    ProductDetailViewModel productDetailViewModel2 = new ProductDetailViewModel()
                    {
                        Product = product2,
                        newProducts = newProducts2
                    };
                    return View(productDetailViewModel2);
                case "Keycap":
                    // Lấy ra các sản phẩm category Keycap và id
                    var product3 = products.Where(p => p.ProCategory == "Keycap" && p.ProId == id).FirstOrDefault();
                    if (product3 == null) // Nếu không có sản phẩm thì chuyển hướng về trang Keycap
                    {
                        return RedirectToAction("Keycap");
                    }
                    // Lấy ra 2 sản phẩm mới nhất
                    var newProducts3 = products.OrderByDescending(p => p.ProId).Take(2).ToList();
                    ProductDetailViewModel productDetailViewModel3 = new ProductDetailViewModel()
                    {
                        Product = product3,
                        newProducts = newProducts3
                    };
                    return View(productDetailViewModel3);
                case "Switch":

                    var product4 = products.Where(p => p.ProCategory == "Switch" && p.ProId == id).FirstOrDefault();
                    if (product4 == null)
                    {
                        return RedirectToAction("Switch");
                    }
                    var newProducts4 = products.OrderByDescending(p => p.ProId).Take(2).ToList();
                    ProductDetailViewModel productDetailViewModel4 = new ProductDetailViewModel()
                    {
                        Product = product4,
                        newProducts = newProducts4
                    };
                    return View(productDetailViewModel4);
                case "Earphone":

                    var product5 = products.Where(p => p.ProCategory == "Earphone" && p.ProId == id).FirstOrDefault();
                    if (product5 == null)
                    {
                        return RedirectToAction("Earphone");
                    }
                    var newProducts5 = products.OrderByDescending(p => p.ProId).Take(2).ToList();
                    ProductDetailViewModel productDetailViewModel5 = new ProductDetailViewModel()
                    {
                        Product = product5,
                        newProducts = newProducts5
                    };
                    return View(productDetailViewModel5);
                default:
                    return RedirectToAction("Index", "Home");

            }

        }

        [ServiceFilter(typeof(UserAuthorizationFilter))]
        public async Task<IActionResult> AddToCart(ProductViewModel pro)
        {
            // check login
            String accJon = HttpContext.Session.GetString("AccGmail");

            if (accJon == null || accJon.Equals(""))
            {
                TempData["Message"] = "login";
                return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
            }
            int quantity = (int)pro.Product.ProQuantity;
            int proId = pro.Product.ProId;
            var productModel = await _productService.GetProductByProID(proId);
            int quantityProduct =  (int)productModel.ProQuantity;
            if (quantity > quantityProduct)
            {
                TempData["Message"] = "ErrorAddtoCart";
                return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
            }
            else
            {
                Account account = JsonConvert.DeserializeObject<Account>(accJon);
                Account acc = await _account.getUserAsync(account.AccGmail);
                User user = acc.User;
                
                // check product in cart
                Cart cart = await _cartService.GetCartByUserIdAndProductId(user.UserId, proId);
                if (cart != null)
                {
                    var newQuantity = cart.CartQuantity + quantity;
                    _cartService.UpdateCart(cart.CartId, newQuantity??0);
                }
                else
                {
                    Cart newCart = new Cart();
                    newCart.ProId = pro.Product.ProId;
                    newCart.CartQuantity = pro.Product.ProQuantity;
                    newCart.UserId = user.UserId;
                    await _cartService.AddCart(newCart);
                    TempData["Message"] = "success";
                    return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
                }
            }
            TempData["Message"] = "success";
            return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
        }

        public async Task<IActionResult> Search(string txt)
        {
            IEnumerable<Product> products = await _productService.SearchProduct(txt);
            return Json(products);
        }

    }
}

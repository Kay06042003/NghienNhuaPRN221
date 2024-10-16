using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using NghienNhuaMVC.Models;
using System.Diagnostics;

namespace NghienNhuaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            // lay ra 8 san pham moi nhat
            IEnumerable<Product> products = await _productService.GetAll();
            products = products.OrderByDescending(p => p.ProId).Take(8).ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

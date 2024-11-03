using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NghienNhuaMVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Thanks()
        {
            return View();
        }

        public IActionResult CreateError(String message)
        {
            ViewData["Message"] = message;
            return View();
        }
    }
}
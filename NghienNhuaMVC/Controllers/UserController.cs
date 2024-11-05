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

namespace NghienNhuaMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountServices _accountServices;
        public UserController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.Session.GetString("AccGmail") ?? string.Empty;
            if(email == "")
            {
                return RedirectToAction("Login", "Account");
            }
            var acc = JsonConvert.DeserializeObject<Account>(email);
            var account = await _accountServices.getUserAsync(acc.AccGmail);
            return View(account);
        }

         public async Task<IActionResult> Edit()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            Account user1 = await _accountServices.getUserAsync(acc.AccGmail);
            return View(user1);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Account account)
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            User user = account.User;
            Account user1 = await _accountServices.getUserAsync(acc.AccGmail);
            user.UserId = user1.User.UserId;
            user = await _accountServices.updateUserAsync(user);
            if (user == null)
            {
                TempData["Message"] = "error";
                return View();
            }
            TempData["Message"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string fullname, string phone, string address)
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            User user = acc.User;
            Account user1 = await _accountServices.getUserAsync(acc.AccGmail);
            user.UserId = user1.User.UserId;
            user.UserFullname = fullname;
            user.UserSdt = phone;
            user.UserAddress = address;
            user = await _accountServices.updateUserAsync(user);
            if (user == null)
            {
                new JsonResult(new { success = false });
            }
            return new JsonResult(new { success = true });
        }


    }
}
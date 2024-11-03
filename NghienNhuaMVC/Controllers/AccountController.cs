using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using NghienNhuaMVC.Services;

namespace NghienNhuaMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISendEmail _sendEmail;
        private readonly IAccountServices _accountServices;
        private readonly IUserServices _userServices;
        public AccountController(ISendEmail sendEmail, IAccountServices accountServices, IUserServices userServices)
        {
            _sendEmail = sendEmail;
            _accountServices = accountServices;
            _userServices = userServices;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string passHash = MD5.MD5Hash(loginViewModel.Account.AccPassword);
                var account = await _accountServices.getAccountAsync(loginViewModel.Account.AccGmail, passHash);
                if (account != null)
                {
                    string role = account.Role;
                    string accountJon = JsonConvert.SerializeObject(account, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    // cookie to save success login and set time 3 days
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(3);
                    option.Path = "/";
                    Response.Cookies.Append("Account", accountJon, option);

                    // session to save success login
                    String accountJonn = JsonConvert.SerializeObject(account, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    HttpContext.Session.SetString("role", role);
                    HttpContext.Session.SetString("Account", account.AccGmail);
                    if (!role.Equals("1"))
                    {
                        return RedirectToAction("Dashboard", "Staff");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Message"] = "error";
            return View(loginViewModel);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection form)
        {
            var email = form["AccGmail"].ToString();
            var pw_hash = MD5.MD5Hash(form["AccPassword"].ToString());
            var fullName = form["UserFullname"].ToString();
            var phone = form["UserSdt"].ToString();
            var address = form["UserAddress"].ToString();

            var account = await _accountServices.getAccountAsync(email, pw_hash);
            if (account != null)
            {
                // sesstion mess error
                TempData["status"] = "error";
                HttpContext.Session.SetString("status", "error");
                return View("Register");
            }
            else
            {
                Account acc = new Account();
                acc.AccGmail = email;
                acc.AccPassword = pw_hash;
                acc.Role = "1";
                User user = new User();
                user.UserFullname = fullName;
                user.UserSdt = phone;
                user.UserAddress = address;
                // send email
                int number = new Random().Next(100000, 999999);
                _sendEmail.SendEmailAsync(email, "Verify email", number, fullName);
                var userJon = JsonConvert.SerializeObject(user);
                var accJon = JsonConvert.SerializeObject(acc);
                // send user and account to next action
                TempData["code"] = number;
                TempData["acc"] = accJon;
                TempData["user"] = userJon;
            }
            return RedirectToAction("VerifyEmail");
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyEmail(IFormCollection form)
        {
            int code = int.Parse(form["code"].ToString());
            int number = int.Parse(TempData["code"].ToString());
            Account acc = JsonConvert.DeserializeObject<Account>(TempData["acc"].ToString());
            User user = JsonConvert.DeserializeObject<User>(TempData["user"].ToString());
            if (code == number)
            {
                _accountServices.addAccount(acc);
                user.AccId = acc.AccId;
                _userServices.addUserAsync(user);
                return RedirectToAction("Login");
            }
            else
            {
                var userJon = JsonConvert.SerializeObject(user);
                var accJon = JsonConvert.SerializeObject(acc);
                // send user and account to next action
                TempData["code"] = number;
                TempData["acc"] = accJon;
                TempData["user"] = userJon;
                TempData["status"] = "error";
                return View();
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(IFormCollection form)
        {
            var email = form["email"].ToString();
            var account = await _accountServices.getAccountAsync(email);
            if (account != null)
            {
                var user = await _userServices.getUserAsync(account.AccId);
                int number = new Random().Next(100000, 999999);
                _sendEmail.SendEmailAsync(email, "Verify email", number, user.UserFullname);
                TempData["code"] = number;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string acc = JsonConvert.SerializeObject(account, settings);
                HttpContext.Session.SetString("AccGmail", acc);
                return RedirectToAction("VerifyForgotPassword");
            }
            else
            {
                TempData["status"] = "error";
                return View();
            }
        }

        public IActionResult VerifyForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyForgotPassword(IFormCollection form)
        {
            int code = int.Parse(form["code"].ToString());
            int number = int.Parse(TempData["code"].ToString());
            if (code == number)
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                TempData["code"] = number;
                TempData["status"] = "error";
                return View();
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(IFormCollection form)
        {
            var password = MD5.MD5Hash(form["password"].ToString());
            var email = HttpContext.Session.GetString("AccGmail");
            var acc = JsonConvert.DeserializeObject<Account>(email);
            if (acc != null)
            {
                acc.AccPassword = password;
                _accountServices.updateAccount(acc);
                // check session login
                TempData["Message"] = "success";
                if (HttpContext.Session.GetString("Account") != null)
                {
                    return RedirectToAction("Index", "User");
                }
                return RedirectToAction("Login");
            }

            TempData["Message"] = "error";
            return View();
        }

        public async Task<IActionResult> VerifyEmailChangePassword()
        {
            var acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("AccGmail"));
            User user = await _userServices.getUserAsync(acc.AccId);
            int number = new Random().Next(100000, 999999);
            _sendEmail.SendEmailAsync(acc.AccGmail, "Verify email", number, user.UserFullname);
            TempData["code"] = number;
            return View("VerifyForgotPassword");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmailChangePassword(IFormCollection form)
        {
            int code = int.Parse(form["code"].ToString());
            int number = int.Parse(TempData["code"].ToString());
            if (code == number)
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                TempData["code"] = number;
                TempData["status"] = "error";
                return View();
            }
        }


        public async Task<IActionResult> Logout()
        {
            // remove session
            HttpContext.Session.Remove("AccGmail");
            // remove cookie
            Response.Cookies.Delete("Account");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CheckAccount()
        {
            string email = TempData["email"] as string;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = await _accountServices.getUserAsync(email);
            if (account != null)
            {
                string role = account.Role;
                string accountJon = JsonConvert.SerializeObject(account, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                // cookie to save success login and set time 3 days
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                HttpContext.Session.SetString("role", role);
                HttpContext.Session.SetString("AccGmail", account.AccGmail);
                if (!role.Equals("1"))
                {
                    return RedirectToAction("Dashboard", "Staff");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["email"] = email;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckAccount(IFormCollection form)
        {
            var email = form["txtEmail"].ToString();
            var fullName = form["txtName"].ToString();
            var phone = form["txtPhone"].ToString();
            var address = form["txtAddress"].ToString();
            var pw_hash = MD5.MD5Hash(form["txtPassword"].ToString());
            var account = await _accountServices.getAccountAsync(email, pw_hash);
            if (account != null)
            {
                string role = account.Role;
                string accountJon = JsonConvert.SerializeObject(account);
                // cookie to save success login and set time 3 days
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                // session to save success login
                HttpContext.Session.SetString("role", role);
                HttpContext.Session.SetString("AccGmail", account.AccGmail);
                if (!role.Equals("1"))
                {
                    return RedirectToAction("Dashboard", "Staff");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Account acc = new Account();
                acc.AccGmail = email;
                acc.AccPassword = pw_hash;
                acc.Role = "1";
                User user = new User();
                user.UserFullname = fullName;
                user.UserSdt = phone;
                user.UserAddress = address;
                _accountServices.addAccount(acc);
                Account account1 = await _accountServices.getAccountAsync(email);
                user.AccId = account1.AccId;
                await _userServices.addUserAsync(user);
                // cookie to save success login and set time 3 days
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string accountJon = JsonConvert.SerializeObject(account1, settings);
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                // session to save success login
                HttpContext.Session.SetString("role", acc.Role);
                HttpContext.Session.SetString("AccGmail", acc.AccGmail);
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
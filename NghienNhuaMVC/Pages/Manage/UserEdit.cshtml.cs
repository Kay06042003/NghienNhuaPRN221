using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Newtonsoft.Json;

namespace NghienNhuaMVC.Pages.Manage
{
    public class UserEditModel : PageModel
    {
        private readonly IAccountServices _accountServices;
        public UserEditModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [BindProperty]
        public User Users { get; set; }
        [BindProperty]
        public string Fullname { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Address { get; set; }
        public async Task<IActionResult> OnGet()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            Account user = await _accountServices.getUserAsync(acc.AccGmail);
            Users = user.User;
            Fullname = user.User.UserFullname;
            Phone = user.User.UserSdt;
            Address = user.User.UserAddress;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _accountServices.updateUserAsync(Users);
            return new JsonResult(new { success = true });
        }

    }
}

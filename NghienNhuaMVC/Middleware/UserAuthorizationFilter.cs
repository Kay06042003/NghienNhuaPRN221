using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NghienNhuaMVC.Middleware
{
    public class UserAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("role") != "1")
            {
                context.HttpContext.Session.SetString("status", "login");
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
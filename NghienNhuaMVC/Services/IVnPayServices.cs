using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NghienNhuaMVC.Models;

namespace NghienNhuaMVC.Services
{
    public interface IVnPayServices
    {
        string CreateRequestUrl(HttpContext context, VnPayResqestModel vnPayment);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collection);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NghienNhuaMVC.Services
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, string subject, int code, string name);
    }
}
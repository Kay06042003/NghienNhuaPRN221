using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAccountServices
    {
        Account addAccount(Account account);
        Account getAccount(int accId);
        Account updateAccount(Account account);

    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Account addAccount(Account account);
        Account getAccount(int accId);
        Account updateAccount(Account account);
    }
}

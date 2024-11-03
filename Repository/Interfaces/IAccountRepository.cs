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
        Task<Account> getAccountAsync(string accountGmail, string accountPassword);
        Task<Account> getUserAsync(string accountGmail);
        Task<Account> getAccountAsync(string accountGmail);
        Task<User> updateUserAsync(User user);
    }
}

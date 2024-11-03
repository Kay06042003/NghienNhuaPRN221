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
        Task<Account> getAccountAsync(string accountGmail, string accountPassword);
        Task<Account> getUserAsync(string accountGmail);
        Task<Account> getAccountAsync(string accountGmail);
        Task<User> updateUserAsync(User user);
        Task<Account> GetAccountByAccGmail(string accGmail);
        Account loginAccount(string username, string password);
        Task<Account> GetById(int id);
        Task Add(Account item);
        Task Update(Account item);
        Task Delete(int id);
        string MD5Hash(string pwd);
    }
}

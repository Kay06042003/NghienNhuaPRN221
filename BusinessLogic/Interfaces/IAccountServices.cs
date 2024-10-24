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
        Task<Account> GetAccountByAccGmail(string accGmail);
        Account loginAccount(string username, string password);
        Task<Account> GetById(int id);
        Task Add(Account item);
        Task Update(Account item);
        Task Delete(int id);
        string MD5Hash(string pwd);
    }
}

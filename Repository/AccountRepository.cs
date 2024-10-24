using DataAccess.DAO;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO accountDAO;

        public AccountRepository()
        {
            accountDAO = new AccountDAO();
        }
        
        public async Task<Account> GetById(int id) => await accountDAO.GetById(id);
        public async Task Add(Account item) => await accountDAO.Add(item);
        public async Task Update(Account item) => await accountDAO.Update(item);
        public async Task Delete(int id) => await accountDAO.Delete(id);
        public async Task<Account> GetAccountByAccGmail(string accGmail) => await accountDAO.GetAccountByAccGmail(accGmail);

        public Account loginAccount(string username, string password)
        {
            return accountDAO.loginAccount(username, password);
        }
        public string MD5Hash(string pwd)
        {
            return accountDAO.HashPasswordWithMD5(pwd);
        }
    }
}

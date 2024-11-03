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
        public Account addAccount(Account account)
        {
            return accountDAO.addAccount(account);
        }

        public Account getAccount(int accId)
        {
            return accountDAO.getAccount(accId);
        }

        public Account updateAccount(Account account)
        {
            return accountDAO.updateAccount(account);
        }
        public async Task<Account> getAccountAsync(string accountGmail, string accountPassword) {
            return await accountDAO.getAccountAsync(accountGmail, accountPassword);
        }

        public async Task<Account> getUserAsync(string accountGmail)
        {
            return await accountDAO.getUserAsync(accountGmail);
        }

        public async Task<Account> getAccountAsync(string accountGmail)
        {
            return await accountDAO.getAccountAsync(accountGmail);
        }

        public async Task<User> updateUserAsync(User user)
        {
            return await accountDAO.updateUserAsync(user);
        }
    }
}

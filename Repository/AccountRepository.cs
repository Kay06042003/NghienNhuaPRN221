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
    }
}

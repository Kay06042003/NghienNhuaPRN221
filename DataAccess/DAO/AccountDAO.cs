using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AccountDAO : SingletonBase<AccountDAO>
    {
        public Account addAccount(Account account)
        {
            var accountExist = _context.Accounts.FirstOrDefault(x => x.AccId == account.AccId);
            if (accountExist == null)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return account;
            }
            return null;
        }

        public Account getAccount(int accId)
        {
            if(accId <= 0)
            {
                return null;
            } 
            return _context.Accounts.FirstOrDefault(x => x.AccId == accId);
        }

        public Account updateAccount(Account account)
        {
            var accountExist = _context.Accounts.FirstOrDefault(x => x.AccId == account.AccId);
            if (accountExist != null)
            {
                accountExist.AccGmail = account.AccGmail;
                accountExist.AccPassword = account.AccPassword;
                _context.SaveChanges();
                return accountExist;
            }
            return null;
        }


    }
}

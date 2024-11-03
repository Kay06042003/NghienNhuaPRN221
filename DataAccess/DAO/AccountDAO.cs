using Microsoft.EntityFrameworkCore;
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
                account.Role = "1";
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return account;
            }
            return null;
        }

        public Account getAccount(int accId)
        {
            if (accId <= 0)
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

        public async Task<Account> getAccountAsync(string accountGmail, string accountPassword)
        {
            return await _context.Accounts
                .Where(x => x.AccGmail == accountGmail && x.AccPassword == accountPassword)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Account> getUserAsync(string accountGmail) {
            return await _context.Accounts
                .Where(x => x.AccGmail == accountGmail)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Account> getAccountAsync(string accountGmail)
        {
            return await _context.Accounts
                .Where(x => x.AccGmail == accountGmail)
                .FirstOrDefaultAsync();
        }

        public async Task<User> updateUserAsync(User user)
        {
            var userExist = _context.Users.FirstOrDefault(x => x.UserId == user.UserId);
            if (userExist != null)
            {
                userExist.UserFullname = user.UserFullname;
                userExist.UserSdt = user.UserSdt;
                userExist.UserAddress = user.UserAddress;
                _context.SaveChanges();
                return userExist;
            }
            return null;
        }


    }
}

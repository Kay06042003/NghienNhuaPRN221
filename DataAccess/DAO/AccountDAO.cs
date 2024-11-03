using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public Account loginAccount(string accGmail, string hashedPassword)
        {
            var account = _context.Accounts
                                 .FirstOrDefault(x => x.AccGmail == accGmail && x.AccPassword == hashedPassword);
            return account;
        }
        public async Task<Account> GetAccountByAccGmail(string accGmail) 
        { 
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccGmail == accGmail);
            if (account == null) return null;
            return account;
        }
        public async Task<Account> GetById(int id)
        {
            var item = await _context.Accounts.FirstOrDefaultAsync(c => c.AccId == id);
            if (item == null) return null;
            return item;
        }

        public async Task Add(Account acc)
        {
            if(acc == null)
                throw new ArgumentNullException(nameof(acc), "Account cannot be null.");
                
            try
            {
                _context.Accounts.Add(acc);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the account to the database.", ex);
            }
        }

        public async Task Update(Account acc)
        {
            var existingItem = await GetById(acc.AccId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(acc);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            try
            {
                var item = await GetById(id);
                if (item != null)
                {
                    _context.Accounts.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi tùy theo yêu cầu của ứng dụng
                throw new Exception($"An error occurred while deleting the account with ID {id}.", ex);
            }
        }
        public string HashPasswordWithMD5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                                // Chuyển đổi byte array sang dạng chuỗi hex
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
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

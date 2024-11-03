using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Models;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IAccountRepository accountRepository;

        public AccountServices(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        public Account addAccount(Account account)
        {
            return accountRepository.addAccount(account);
        }

        public Account getAccount(int accId)
        {
            return accountRepository.getAccount(accId);
        }

        public Account updateAccount(Account account)
        {
            return accountRepository.updateAccount(account);
        }

        public async Task<Account> getAccountAsync(string accountGmail, string accountPassword) {
            return await accountRepository.getAccountAsync(accountGmail, accountPassword);
        }

        public async Task<Account> getUserAsync(string accountGmail)
        {
            return await accountRepository.getUserAsync(accountGmail);
        }

        public Task<Account> getAccountAsync(string accountGmail)
        {
           return accountRepository.getAccountAsync(accountGmail);
        }

        public async Task<User> updateUserAsync(User user)
        {
            return await accountRepository.updateUserAsync(user);
        }
    }
}

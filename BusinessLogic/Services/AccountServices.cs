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
        private readonly IAccountRepository _accountRepository;

        public AccountServices(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Add(Account item)
        {
            await _accountRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _accountRepository.Delete(id);
        }

        public async Task<Account> GetAccountByAccGmail(string accGmail)
        {
            return await _accountRepository.GetAccountByAccGmail(accGmail);
        }

        public async Task<Account> GetById(int id)
        {
            return await _accountRepository.GetById(id);
        }

        public Account loginAccount(string username, string password)
        {
            return _accountRepository.loginAccount(username, password);
        }

        public string MD5Hash(string pwd)
        {
            return _accountRepository.MD5Hash(pwd);
        }

        public async Task Update(Account item)
        {
             await _accountRepository.Update(item);
        }

    }
}

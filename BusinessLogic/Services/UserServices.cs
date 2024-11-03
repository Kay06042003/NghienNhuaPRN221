using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Models;
using Repository.Interfaces;
using BusinessLogic.Interfaces;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task addUserAsync(User user)
        {
            await _userRepository.addUserAsync(user);
        }

        public async Task<User> getUserAsync(int accId)
        {
            return await _userRepository.getUserAsync(accId);
        }

        public async Task updateUserAsync(User user)
        {
            await _userRepository.updateUserAsync(user);
        }
        public async Task<User> GetUserByAccId(int id) 
        { 
            return  await _userRepository.GetUserByAccId(id);
        }
    }
}


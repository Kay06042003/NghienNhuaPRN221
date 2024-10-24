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
        private readonly IUserRepository _userRepo;

        public UserServices(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public async Task<User> GetUserByAccId(int id) 
        { 
            return  await _userRepo.GetUserByAccId(id);
        }
    }
}

using DataAccess.DAO;
using Models;
using Repository.Interfaces;
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
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO userDAO;
        public UserRepository()
        {
            userDAO = new UserDAO();
        }
        public async Task addUserAsync(User user)
        {
            await userDAO.addUserAsync(user);
        }

        public async Task<User> getUserAsync(int accId)
        {
           return await userDAO.getUserAsync(accId);
        }

        public async Task updateUserAsync(User user)
        {
            await userDAO.updateUserAsync(user);
        }

        public UserRepository(UserDAO _userDAO)
        {
            userDAO = _userDAO;
        }

        public async Task<User> GetUserByAccId(int id) =>  await userDAO.getUserByAccID(id);
        
    }
}


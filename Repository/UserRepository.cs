using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DAO;
using Models;
using Repository.Interfaces;

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
    }
}
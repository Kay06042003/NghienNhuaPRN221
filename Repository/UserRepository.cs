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

        public UserRepository(UserDAO _userDAO)
        {
            userDAO = _userDAO;
        }

        public async Task<User> GetUserByAccId(int id) =>  await userDAO.getUserByAccID(id);
        
    }
}

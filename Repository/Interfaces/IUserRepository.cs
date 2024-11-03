using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> getUserAsync(int accId);
        Task addUserAsync(User user);
        Task updateUserAsync(User user);
        Task<User> GetUserByAccId(int id);
    }
}

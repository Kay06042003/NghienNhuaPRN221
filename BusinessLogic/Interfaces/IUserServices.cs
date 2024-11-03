using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BusinessLogic.Interfaces
{
    public interface IUserServices
    {
        Task<User> getUserAsync(int accId);
        Task addUserAsync(User user);
        Task updateUserAsync(User user);
    }
}
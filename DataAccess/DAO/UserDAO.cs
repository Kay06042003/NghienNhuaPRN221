using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class UserDAO : SingletonBase<UserDAO>
    {
        public User addUser(User user)
        {
            var userExist = _context.Users.FirstOrDefault(x => x.AccId == user.AccId);
            if (userExist == null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            return null;
        }

        public async Task<User> getUserByAccID(int accId)
        {
            if (accId <= 0)
            {
                return null;
            }
            var user  = await _context.Users.FirstOrDefaultAsync(x => x.AccId == accId);
            if(user == null)
            {
                return null;
            }
            return user;
        }

        /*public User updateUser(User user)
        {
            var userExist = _context.Accounts.FirstOrDefault(x => x.AccId == user.AccId);
            if (userExist != null)
            {
                userExist.AccGmail = user.;
                userExist.AccPassword = user.AccPassword;
                _context.SaveChanges();
                return accountExist;
            }
            return null;
        }*/
    }
}

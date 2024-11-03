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

        public async Task<User> getUserAsync(int accId)
        {
            return await _context.Users
                .Where(x => x.AccId == accId)
                .FirstOrDefaultAsync();
        }

        // add user async
        public async Task addUserAsync(User user)
        {
            await _context.SaveChangesAsync();
        }
        // update user async
        public async Task updateUserAsync(User user)
        {
            var userExist = _context.Users.FirstOrDefault(x => x.AccId == user.AccId);
            if (userExist != null)
            {
                userExist.UserFullname = user.UserFullname;
                userExist.UserAddress = user.UserAddress;
                userExist.UserSdt = user.UserSdt;
                await _context.SaveChangesAsync();
            }
        }

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
    }
}

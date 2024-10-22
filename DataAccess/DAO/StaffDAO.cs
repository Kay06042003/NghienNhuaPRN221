using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class StaffDAO : SingletonBase<StaffDAO>
    {
        public async Task<IEnumerable<Staff>> GetListAll()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<Staff> GetById(int id)
        {
            var item = await _context.Staffs.FirstOrDefaultAsync(c => c.StaffId == id);
            if (item == null) return null;
            return item;
        }

        public async Task Add(Staff item)
        {
            _context.Staffs.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Staff item)
        {
            var existingItem = await GetById(item.StaffId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var staff = await GetById(id);
            if (staff != null)
            {
                staff.StaffStatus = "Tired";
                _context.Staffs.Update(staff);
                await _context.SaveChangesAsync();
            }
        }
    }
}

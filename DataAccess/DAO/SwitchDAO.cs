using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SwitchDAO : SingletonBase<SwitchDAO>
    {
        public async Task<IEnumerable<Switch>> GetListALl()
        {
            return await _context.Switches.ToListAsync();
        }

        public async Task<Switch> GetById(int id)
        {
            return await _context.Switches.FindAsync(id);
        }


        public async Task Add(Switch sw)
        {
            _context.Switches.Add(sw);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Switch sw)
        {
            var switchUpdate = await GetById(sw.SwitchId);
            if (switchUpdate == null)
            {
                throw new Exception("Switch not found");
            }
            _context.Switches.Update(sw);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var sw = await GetById(id);
            if (sw == null)
            {
                throw new Exception("Switch not found");
            }
            _context.Switches.Remove(sw);
            await _context.SaveChangesAsync();
        }
    }
}

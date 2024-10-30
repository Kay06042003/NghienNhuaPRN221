using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class KeycapDAO : SingletonBase<KeycapDAO>
    {
        public async Task<IEnumerable<Keycap>> GetListALl()
        {
            return await _context.Keycaps.Include(k => k.Pro).ToListAsync();
        }

        public async Task<Keycap> GetById(int id)
        {
            return await _context.Keycaps.FindAsync(id);
        }


        public async Task Add(Keycap keycap)
        {
            _context.Keycaps.Add(keycap);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Keycap keycap)
        {
            var keycapUpdate = await GetById(keycap.KcId);
            if (keycapUpdate == null)
            {
                throw new Exception("Keycap not found");
            }
            _context.Keycaps.Update(keycap);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var keycap = await GetById(id);
            if (keycap == null)
            {
                throw new Exception("Keycap not found");
            }
            _context.Keycaps.Remove(keycap);
            await _context.SaveChangesAsync();
        }
    }
}

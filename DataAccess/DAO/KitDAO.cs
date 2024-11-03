using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class KitDAO : SingletonBase<KitDAO>
    {
        public async Task<IEnumerable<Kit>> GetListALl()
        {
            return await _context.Kits.Include(k => k.Pro).ToListAsync();
        }

        public async Task<Kit> GetById(int id)
        {
            return await _context.Kits.FindAsync(id);
        }


        public async Task Add(Kit kit)
        {
            _context.Kits.Add(kit);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Kit kit)
        {
            var kitUpdate = await GetById(kit.KitId);
            if (kitUpdate == null)
            {
                throw new Exception("Kit not found");
            }
            _context.Kits.Update(kit);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var kit = await GetById(id);
            if (kit == null)
            {
                throw new Exception("Kit not found");
            }
            _context.Kits.Remove(kit);
            await _context.SaveChangesAsync();
        }
    }
}

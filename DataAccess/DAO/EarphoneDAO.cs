using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class EarphoneDAO : SingletonBase<EarphoneDAO>
    {
         public async Task<IEnumerable<Earphone>> GetListALl()
        {
            return await _context.Earphones.Include(e => e.Pro).ToListAsync();
        }

        public async Task<Earphone> GetById(int id)
        {
            return await _context.Earphones.FindAsync(id);
        }


        public async Task Add(Earphone earphone)
        {
            _context.Earphones.Add(earphone);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Earphone earphone)
        {
            var earphoneUpdate = await GetById(earphone.EarId);
            if (earphoneUpdate == null)
            {
                throw new Exception("Earphone not found");
            }
            _context.Earphones.Update(earphone);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var earphone = await GetById(id);
            if (earphone == null)
            {
                throw new Exception("Earphone not found");
            }
            _context.Earphones.Remove(earphone);
            await _context.SaveChangesAsync();
        }
    }
}

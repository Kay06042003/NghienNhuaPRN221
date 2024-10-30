using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MouseDAO : SingletonBase<MouseDAO>
    {
        public async Task<IEnumerable<Mouse>> GetListALl()
        {
            return await _context.Mice.Include(m => m.Pro).ToListAsync();
        }

        public async Task<Mouse> GetById(int id)
        {
            return await _context.Mice.FindAsync(id);
        }


        public async Task Add(Mouse mouse)
        {
            _context.Mice.Add(mouse);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Mouse mouse)
        {
            var mouseUpdate = await GetById(mouse.MouseId);
            if (mouseUpdate == null)
            {
                throw new Exception("Mouse not found");
            }
            _context.Mice.Update(mouse);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var mouse = await GetById(id);
            if (mouse == null)
            {
                throw new Exception("Mouse not found");
            }
            _context.Mice.Remove(mouse);
            await _context.SaveChangesAsync();
        }
    }
}

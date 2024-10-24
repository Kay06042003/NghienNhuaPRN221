using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class KeyboardDAO : SingletonBase<KeyboardDAO>
    {
        public async Task<IEnumerable<KeyBoard>> GetListALl()
        {
            return await _context.KeyBoards.ToListAsync();
        }

        public async Task<KeyBoard> GetById(int id)
        {
            return await _context.KeyBoards.FindAsync(id);
        }


        public async Task Add(KeyBoard keyboard)
        {
            _context.KeyBoards.Add(keyboard);
            await _context.SaveChangesAsync();
        }

        public async Task Update(KeyBoard keyboard)
        {
            var keyboardUpdate = await GetById(keyboard.KbId);
            if (keyboardUpdate == null)
            {
                throw new Exception("Keyboard not found");
            }
            _context.KeyBoards.Update(keyboard);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var keyboard = await GetById(id);
            if (keyboard == null)
            {
                throw new Exception("Keyboard not found");
            }
            _context.KeyBoards.Remove(keyboard);
            await _context.SaveChangesAsync();
        }
    }
}

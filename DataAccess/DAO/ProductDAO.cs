using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAO
{
    public class ProductDAO : SingletonBase<ProductDAO>
    {
        public async Task<IEnumerable<Product>> GetAll() {
            return await _context.Products.
            Include(p => p.Earphone).
            Include(p => p.KeyBoard).
            Include(p => p.Keycap).
            Include(p => p.Kit).
            Include(p => p.Mouse).
            Include(p => p.Switch).
            ToListAsync();
        }

        public async Task<Product> GetProductByProID(int proId) {
            return await _context.Products
                .Include(p => p.Earphone)
                .Include(p => p.KeyBoard)
                .Include(p => p.Keycap)
                .Include(p => p.Kit)
                .Include(p => p.Mouse)
                .Include(p => p.Switch)
                .FirstOrDefaultAsync(p => p.ProId == proId);
        }

        public async Task<IEnumerable<Product>> SearchProduct(string txt)
        {
            return await _context.Products.Where(p => p.ProName.Contains(txt)).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetListAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            var item = await _context.Products.FirstOrDefaultAsync(c => c.ProId == id);
            if (item == null) return null;
            return item;
        }

        public async Task Add(Product item)
        {
            _context.Products.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product item)
        {
            var existingItem = await GetById(item.ProId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(item);
            }
            await _context.SaveChangesAsync();
        }
    }
}

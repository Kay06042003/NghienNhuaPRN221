using System;
using System.Collections.Generic;
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
    }
}

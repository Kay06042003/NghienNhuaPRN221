using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetProductByProID(int proId);
        Task<IEnumerable<Product>> SearchProduct(string txt);
        Task<IEnumerable<Product>> GetListAll();
        Task<Product> GetById(int id);
        Task Add(Product item);
        Task Update(Product item);
    }
}

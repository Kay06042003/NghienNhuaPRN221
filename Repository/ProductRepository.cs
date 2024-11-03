using DataAccess.DAO;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        public readonly ProductDAO _productDAO;
        public ProductRepository(ProductDAO productDAO) {
            _productDAO = productDAO;
        }
        public Task<IEnumerable<Product>> GetAll()
        {
            return _productDAO.GetAll();   
        }

        public async Task<Product> GetProductByProID(int proId)
        {
            return await _productDAO.GetProductByProID(proId);
        }

        public async Task<IEnumerable<Product>> SearchProduct(string txt)
        {
            return await _productDAO.SearchProduct(txt);
        }

        public async Task<IEnumerable<Product>> GetListAll() => await _productDAO.GetListAll();
        public async Task<Product> GetById(int id) => await _productDAO.GetById(id);
        public async Task Add(Product item) => await _productDAO.Add(item);
        public async Task Update(Product item) => await _productDAO.Update(item);

    }
}

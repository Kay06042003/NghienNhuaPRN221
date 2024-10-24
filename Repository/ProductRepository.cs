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
    }
}

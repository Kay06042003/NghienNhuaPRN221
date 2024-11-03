using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository;

namespace BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task<IEnumerable<Product>> GetAll()
        {
            return _productRepository.GetAll();
        }
        
        public async Task<Product> GetProductByProID(int proId)
        {
            return await _productRepository.GetProductByProID(proId);
        }

        public async Task<IEnumerable<Product>> SearchProduct(string txt)
        {
            return await _productRepository.SearchProduct(txt);
        }

        public async Task<IEnumerable<Product>> GetListAllProduct()
        {
            return await _productRepository.GetListAll();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task AddProduct(Product pro)
        {
            // Thêm logic nghiệp vụ nếu cần
            await _productRepository.Add(pro);
        }

        public async Task UpdateProduct(Product pro)
        {
            // Thêm logic nghiệp vụ nếu cần
            await _productRepository.Update(pro);
        }
    }
}

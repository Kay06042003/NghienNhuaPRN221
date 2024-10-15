using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BusinessLogic.ViewModels
{
    public class ProductDetailViewModel
    {
        public IEnumerable<Product> newProducts { get; set; }
        public Product Product { get; set; }
        
    }
}
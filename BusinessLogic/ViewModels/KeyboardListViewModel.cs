using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace BusinessLogic.ViewModels
{
    public class KeyboardListViewModel
    {
        public IEnumerable<Product> newProducts { get; set; } 
        public IEnumerable<Product> Products { get; set; }
    }
}
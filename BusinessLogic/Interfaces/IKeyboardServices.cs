using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IKeyboardServices
    {
        Task<IEnumerable<KeyBoard>> GetListAll();
        Task<KeyBoard> GetById(int id);
        Task Add(KeyBoard item);
        Task Update(KeyBoard item);
        Task Delete(int id);
    }
}

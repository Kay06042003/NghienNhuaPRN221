using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMouseServices
    {
        Task<IEnumerable<Mouse>> GetListAll();
        Task<Mouse> GetById(int id);
        Task Add(Mouse item);
        Task Update(Mouse item);
        Task Delete(int id);
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMouseRepository
    {
        Task<IEnumerable<Mouse>> GetListAll();
        Task<Mouse> GetById(int id);
        Task Add(Mouse item);
        Task Update(Mouse item);
        Task Delete(int id);
    }
}

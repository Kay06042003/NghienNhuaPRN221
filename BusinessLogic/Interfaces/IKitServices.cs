using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IKitServices
    {
        Task<IEnumerable<Kit>> GetListAll();
        Task<Kit> GetById(int id);
        Task Add(Kit item);
        Task Update(Kit item);
        Task Delete(int id);
    }
}

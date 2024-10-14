using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IKitRepository
    {
        Task<IEnumerable<Kit>> GetListAll();
        Task<Kit> GetById(int id);
        Task Add(Kit item);
        Task Update(Kit item);
        Task Delete(int id);
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IKeycapRepository
    {
        Task<IEnumerable<Keycap>> GetListAll();
        Task<Keycap> GetById(int id);
        Task Add(Keycap item);
        Task Update(Keycap item);
        Task Delete(int id);
    }
}

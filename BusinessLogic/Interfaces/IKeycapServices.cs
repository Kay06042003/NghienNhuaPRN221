using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IKeycapServices
    {
        Task<IEnumerable<Keycap>> GetListAll();
        Task<Keycap> GetById(int id);
        Task Add(Keycap item);
        Task Update(Keycap item);
        Task Delete(int id);
    }
}

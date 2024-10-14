using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ISwitchRepository
    {
        Task<IEnumerable<Switch>> GetListAll();
        Task<Switch> GetById(int id);
        Task Add(Switch item);
        Task Update(Switch item);
        Task Delete(int id);
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IEarphoneServices
    {
        Task<IEnumerable<Earphone>> GetListAll();
        Task<Earphone> GetById(int id);
        Task Add(Earphone item);
        Task Update(Earphone item);
        Task Delete(int id);
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetListAll();
        Task<Staff> GetById(int id);
        Task Add(Staff item);
        Task Update(Staff item);
        Task Delete(int id);
        Task Recover(int id);
        Task<Staff> GetByAccId(int id);
        Task<Staff> GetByAccGmail(string accGmail);
    }
}

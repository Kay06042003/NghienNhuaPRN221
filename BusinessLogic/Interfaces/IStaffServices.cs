using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IStaffServices
    {
        Task<IEnumerable<Staff>> GetListAllStaff();
        Task<Staff> GetByIdStaff(int id);
        Task AddStaff(Staff item);
        Task UpdateStaff(Staff item);
        Task DeleteStaff(int id);
    }
}

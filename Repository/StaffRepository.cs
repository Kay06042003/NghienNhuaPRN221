using DataAccess.DAO;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StaffRepository : IStaffRepository
    {
        private StaffDAO _staffDAO;
        public StaffRepository(StaffDAO staffDAO)
        {
            _staffDAO = staffDAO;
        }
        public async Task<IEnumerable<Staff>> GetListAll() => await _staffDAO.GetListAll();
        public async Task<Staff> GetById(int id) => await _staffDAO.GetById(id);
        public async Task Add(Staff item) => await _staffDAO.Add(item);
        public async Task Update(Staff item) => await _staffDAO.Update(item);
        public async Task Delete(int id) => await _staffDAO.Delete(id);
    }
}

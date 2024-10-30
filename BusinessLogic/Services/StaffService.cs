using BusinessLogic.Interfaces;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class StaffService :IStaffServices
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }
    
        public async Task<IEnumerable<Staff>> GetListAllStaff()
        {
            return await _staffRepository.GetListAll();
        }

        public async Task<Staff> GetByIdStaff(int id)
        {
            return await _staffRepository.GetById(id);
        }

        public async Task AddStaff(Staff student)
        {
            // Thêm logic nghiệp vụ nếu cần
            await _staffRepository.Add(student);
        }

        public async Task UpdateStaff(Staff student)
        {
            // Thêm logic nghiệp vụ nếu cần
            await _staffRepository.Update(student);
        }

        public async Task DeleteStaff(int id)
        {
            // Thêm logic nghiệp vụ nếu cần
            await _staffRepository.Delete(id);
        }

        public async Task<Staff> GetByAccId(int id)
        {
            return await _staffRepository.GetByAccId(id);
        }

        public async Task RecoverStaff(int id)
        {
            await _staffRepository.Recover(id);
        }

        public async Task<Staff> GetByAccGmail(string accGmail)
        {
            return await _staffRepository.GetByAccGmail(accGmail);
        }
    }
}

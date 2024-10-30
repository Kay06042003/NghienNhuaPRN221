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
    public class MouseServices : IMouseServices
    {
        private IMouseRepository _mouseRepository;

        public MouseServices( IMouseRepository mouseRepository)
        {
            _mouseRepository = mouseRepository;
        }

        public async Task Add(Mouse item)
        {
            await _mouseRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _mouseRepository.Delete(id);
        }

        public async Task<Mouse> GetById(int id)
        {
            return await _mouseRepository.GetById(id);
        }

        public async Task<IEnumerable<Mouse>> GetListAll()
        {
            return await _mouseRepository.GetListAll();
        }

        public async Task Update(Mouse item)
        {
            await _mouseRepository.Update(item);
        }
    }
}

using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MouseServices
    {
        private IMouseServices _mouseServices;

        public MouseServices(IMouseServices mouseServices)
        {
            _mouseServices = mouseServices;
        }

        public async Task Add(Mouse item)
        {
            await _mouseServices.Add(item);
        }

        public async Task Delete(int id)
        {
            await _mouseServices.Delete(id);
        }

        public async Task<Mouse> GetById(int id)
        {
            return await _mouseServices.GetById(id);
        }

        public async Task<IEnumerable<Mouse>> GetListAll()
        {
            return await _mouseServices.GetListAll();
        }

        public async Task Update(Mouse item)
        {
            await _mouseServices.Update(item);
        }
    }
}

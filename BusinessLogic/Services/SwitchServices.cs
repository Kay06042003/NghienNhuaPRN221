using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class SwitchServices
    {
        private ISwitchServices _switchServices;

        public SwitchServices(ISwitchServices switchServices)
        {
            _switchServices = switchServices;
        }

        public async Task Add(Switch item)
        {
            await _switchServices.Add(item);
        }

        public async Task Delete(int id)
        {
            await _switchServices.Delete(id);
        }

        public async Task<Switch> GetById(int id)
        {
            return await _switchServices.GetById(id);
        }

        public async Task<IEnumerable<Switch>> GetListAll()
        {
            return await _switchServices.GetListAll();
        }

        public async Task Update(Switch item)
        {
            await _switchServices.Update(item);
        }
    }
}

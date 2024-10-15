using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class KeycapServices
    {
        private IKeycapServices _keycapServices;

        public KeycapServices(IKeycapServices keycapServices)
        {
            _keycapServices = keycapServices;
        }

        public async Task Add(Keycap item)
        {
            await _keycapServices.Add(item);
        }

        public async Task Delete(int id)
        {
            await _keycapServices.Delete(id);
        }

        public async Task<Keycap> GetById(int id)
        {
            return await _keycapServices.GetById(id);
        }

        public async Task<IEnumerable<Keycap>> GetListAll()
        {
            return await _keycapServices.GetListAll();
        }

        public async Task Update(Keycap item)
        {
            await _keycapServices.Update(item);
        }
    }
}

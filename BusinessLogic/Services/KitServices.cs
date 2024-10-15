using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class KitServices
    {
        private IKitServices _kitServices;

        public KitServices(IKitServices kitServices)
        {
            _kitServices = kitServices;
        }

        public async Task Add(Kit item)
        {
            await _kitServices.Add(item);
        }

        public async Task Delete(int id)
        {
            await _kitServices.Delete(id);
        }

        public async Task<Kit> GetById(int id)
        {
            return await _kitServices.GetById(id);
        }

        public async Task<IEnumerable<Kit>> GetListAll()
        {
            return await _kitServices.GetListAll();
        }

        public async Task Update(Kit item)
        {
            await _kitServices.Update(item);
        }
    }
}

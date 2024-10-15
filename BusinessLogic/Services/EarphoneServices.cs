

using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class EarphoneServices
    {
        private IEarphoneServices _earphoneServices;

        public EarphoneServices(IEarphoneServices earphoneServices)
        {
            _earphoneServices = earphoneServices;
        }

        public async Task Add(Earphone item)
        {
            await _earphoneServices.Add(item);
        }

        public async Task Delete(int id)
        {
            await _earphoneServices.Delete(id);
        }

        public async Task<Earphone> GetById(int id)
        {
            return await _earphoneServices.GetById(id);
        }

        public async Task<IEnumerable<Earphone>> GetListAll()
        {
            return await _earphoneServices.GetListAll();
        }

        public async Task Update(Earphone item)
        {
            await _earphoneServices.Update(item);
        }
    }
}

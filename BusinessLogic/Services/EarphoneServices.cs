

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
    public class EarphoneServices : IEarphoneServices
    {
        private IEarphoneRepository _earphoneRepository;

        public EarphoneServices(IEarphoneRepository earphoneRepository)
        {
            _earphoneRepository = earphoneRepository;
        }

        public async Task Add(Earphone item)
        {
            await _earphoneRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _earphoneRepository.Delete(id);
        }

        public async Task<Earphone> GetById(int id)
        {
            return await _earphoneRepository.GetById(id);
        }

        public async Task<IEnumerable<Earphone>> GetListAll()
        {
            return await _earphoneRepository.GetListAll();
        }

        public async Task Update(Earphone item)
        {
            await _earphoneRepository.Update(item);
        }
    }
}

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
    public class KitServices : IKitServices
    {
        private IKitRepository _kitRepository;

        public KitServices(IKitRepository kitRepository)
        {
            _kitRepository = kitRepository;
        }

        public async Task Add(Kit item)
        {
            await _kitRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _kitRepository.Delete(id);
        }

        public async Task<Kit> GetById(int id)
        {
            return await _kitRepository.GetById(id);
        }

        public async Task<IEnumerable<Kit>> GetListAll()
        {
            return await _kitRepository.GetListAll();
        }

        public async Task Update(Kit item)
        {
            await _kitRepository.Update(item);
        }
    }
}

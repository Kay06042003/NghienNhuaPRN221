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
    public class KeycapServices : IKeycapServices
    {
        private IKeycapRepository _keycapRepository;

        public KeycapServices(IKeycapRepository keycapRepository)
        {
            _keycapRepository = keycapRepository;
        }

        public async Task Add(Keycap item)
        {
            await _keycapRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _keycapRepository.Delete(id);
        }

        public async Task<Keycap> GetById(int id)
        {
            return await _keycapRepository.GetById(id);
        }

        public async Task<IEnumerable<Keycap>> GetListAll()
        {
            return await _keycapRepository.GetListAll();
        }

        public async Task Update(Keycap item)
        {
            await _keycapRepository.Update(item);
        }
    }
}

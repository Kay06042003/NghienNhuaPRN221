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
    public class SwitchServices : ISwitchServices
    {
        private ISwitchRepository _switchRepository;

        public SwitchServices(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task Add(Switch item)
        {
            await _switchRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _switchRepository.Delete(id);
        }

        public async Task<Switch> GetById(int id)
        {
            return await _switchRepository.GetById(id);
        }

        public async Task<IEnumerable<Switch>> GetListAll()
        {
            return await _switchRepository.GetListAll();
        }

        public async Task Update(Switch item)
        {
            await _switchRepository.Update(item);
        }
    }
}

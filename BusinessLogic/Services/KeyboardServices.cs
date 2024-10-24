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
    public class KeyboardServices : IKeyboardServices
    {
        private IKeyboardRepository _keyboardrepository;

        public KeyboardServices(IKeyboardRepository keyboardrepository)
        {
            _keyboardrepository = keyboardrepository;
        }

        public async Task Add(KeyBoard item)
        {
            await _keyboardrepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _keyboardrepository.Delete(id);
        }

        public async Task<KeyBoard> GetById(int id)
        {
            return await _keyboardrepository.GetById(id);
        }

        public async Task<IEnumerable<KeyBoard>> GetListAll()
        {
            return await _keyboardrepository.GetListAll();
        }

        public async Task Update(KeyBoard item)
        {
            await _keyboardrepository.Update(item);
        }
    }
}

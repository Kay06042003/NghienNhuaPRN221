using BusinessLogic.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class KeyboardServices : IKeyboardServices
    {
        private IKeyboardServices _keyboardServices;

        public KeyboardServices(IKeyboardServices keyboardServices)
        {
            _keyboardServices = keyboardServices;
        }

        public async Task Add(KeyBoard item)
        {
            await _keyboardServices.Add(item);
        }

        public async Task Delete(int id)
        {
            await _keyboardServices.Delete(id);
        }

        public async Task<KeyBoard> GetById(int id)
        {
            return await _keyboardServices.GetById(id);
        }

        public async Task<IEnumerable<KeyBoard>> GetListAll()
        {
            return await _keyboardServices.GetListAll();
        }

        public async Task Update(KeyBoard item)
        {
            await _keyboardServices.Update(item);
        }
    }
}

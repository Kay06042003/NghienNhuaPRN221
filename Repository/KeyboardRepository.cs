using DataAccess.DAO;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class KeyboardRepository : IKeyboardRepository
    {
        private KeyboardDAO keyboardDAO;
        public KeyboardRepository(KeyboardDAO keyboardDAO)
        {
            this.keyboardDAO = keyboardDAO;
        }

        public async Task Add(KeyBoard item)
        {
            await keyboardDAO.Add(item);
        }

        public async Task Delete(int id)
        {
            await keyboardDAO.Delete(id);
        }

        public async Task<KeyBoard> GetById(int id)
        {
            return await keyboardDAO.GetById(id);
        }

        public async Task<IEnumerable<KeyBoard>> GetListAll()
        {
            return await keyboardDAO.GetListALl();
        }

        public async Task Update(KeyBoard item)
        {
            await keyboardDAO.Update(item);
        }
    }
}

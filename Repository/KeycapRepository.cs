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
    public class KeycapRepository : IKeycapRepository
    {
        private KeycapDAO keycapDAO;
        public KeycapRepository(KeycapDAO keycapDAO)
        {
            this.keycapDAO = keycapDAO;
        }

        public async Task Add(Keycap item)
        {
            await keycapDAO.Add(item);
        }

        public async Task Delete(int id)
        {
            await keycapDAO.Delete(id);
        }

        public async Task<Keycap> GetById(int id)
        {
            return await keycapDAO.GetById(id);
        }

        public Task<IEnumerable<Keycap>> GetListAll()
        {
            return keycapDAO.GetListALl();
        }

        public async Task Update(Keycap item)
        {
            await keycapDAO.Update(item);
        }
    }
}

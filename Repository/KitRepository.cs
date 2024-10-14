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
    public class KitRepository : IKitRepository
    {
        private KitDAO kitDAO;
        public KitRepository(KitDAO kitDAO)
        {
            this.kitDAO = kitDAO;
        }

        public async Task Add(Kit item)
        {
            await kitDAO.Add(item);
        }

        public async Task Delete(int id)
        {
            await kitDAO.Delete(id);
        }

        public async Task<Kit> GetById(int id)
        {
            return await kitDAO.GetById(id);
        }

        public Task<IEnumerable<Kit>> GetListAll()
        {
            return kitDAO.GetListALl();
        }

        public async Task Update(Kit item)
        {
            await kitDAO.Update(item);
        }
    }
}

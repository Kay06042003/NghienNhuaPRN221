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
    public class EarphoneRepository : IEarphoneRepository
    {
        private EarphoneDAO earphoneDAO;
        public EarphoneRepository(EarphoneDAO earphoneDAO)
        {
            this.earphoneDAO = earphoneDAO;
        }

        public async Task Add(Earphone item)
        {
            await earphoneDAO.Add(item);
        }

        public async Task Delete(int id)
        {
            await earphoneDAO.Delete(id);
        }

        public async Task<Earphone> GetById(int id)
        {
            return await earphoneDAO.GetById(id);
        }

        public Task<IEnumerable<Earphone>> GetListAll()
        {
            return earphoneDAO.GetListALl();
        }

        public async Task Update(Earphone item)
        {
            await earphoneDAO.Update(item);
        }
    }
}

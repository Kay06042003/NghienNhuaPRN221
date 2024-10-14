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
    public  class SwitchRepository : ISwitchRepository
    {
        private SwitchDAO switchDAO;
        public SwitchRepository(SwitchDAO switchDAO)
        {
            this.switchDAO = switchDAO;
        }

        public async Task Add(Switch item)
        {
            await switchDAO.Add(item);
        }

        public async Task Delete(int id)
        {
            await switchDAO.Delete(id);
        }

        public async Task<Switch> GetById(int id)
        {
            return await switchDAO.GetById(id);
        }

        public Task<IEnumerable<Switch>> GetListAll()
        {
            return switchDAO.GetListALl();
        }

        public async Task Update(Switch item)
        {
            await switchDAO.Update(item);
        }
    }
}

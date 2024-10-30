using DataAccess.DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MouseRepository : IMouseRepository
    {
        private MouseDAO mouseDAO;
        public MouseRepository(MouseDAO mouseDAO)
        {
            this.mouseDAO = mouseDAO;
        }

        public async Task Add(Mouse item)
        {
            await mouseDAO.Add(item);
        }

        public async Task Delete(int id)
        {
            await mouseDAO.Delete(id);
        }

        public async Task<Mouse> GetById(int id)
        {
            return await mouseDAO.GetById(id);
        }

        public async Task<IEnumerable<Mouse>> GetListAll()
        {
            return await mouseDAO.GetListALl();
        }

        public async Task Update(Mouse item)
        {
            await mouseDAO.Update(item);
        }
    }
}

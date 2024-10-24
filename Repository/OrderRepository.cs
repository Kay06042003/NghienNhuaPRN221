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
    public class OrderRepository : IOrderRepository
    {
        private OrderDAO _orderDAO;

        public OrderRepository(OrderDAO orderDAO) 
        {
            _orderDAO = orderDAO;
        }

        public Task Approve(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetListOrderConfirm() => await _orderDAO.GetListOrderConfirm();

        public Task Reject(int id)
        {
            throw new NotImplementedException();
        }
    }
}

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

        public async Task Approve(int id)
        {
            await _orderDAO.Approve(id);
        }

        public async Task<Order> GetById(int id)
        {
            return await _orderDAO.GetById(id);
        }

        public async Task<IEnumerable<Order>> GetListOrderConfirm() => await _orderDAO.GetListOrderConfirm();

        public async Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id)
        {
            return await _orderDAO.GetListOrderDetail(id);
        }

        public async Task<IEnumerable<Order>> GetListOrderUpdate()
        {
            return await _orderDAO.GetListOrderUpdate();
        }

        public async Task Reject(int id)
        {
            await _orderDAO.Reject(id);
        }

        public async Task Update(Order item)
        {
            await _orderDAO.Update(item);
        }
    }
}

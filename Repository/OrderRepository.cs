using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DAO;
using Models;
using Repository.Interfaces;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;
        public OrderRepository(OrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }
        public async Task addOrder(Order order)
        {
            await _orderDAO.addOrder(order);
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            await _orderDAO.AddOrderDetail(orderDetail);
        }

        public Task Approve(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetListOrderConfirm()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> getOrder(int orderId)
        {
           return await _orderDAO.getOrder(orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _orderDAO.GetOrderDetails(orderId);
        }

        public async Task<IEnumerable<Order>> getOrders(int userID)
        {
            return await _orderDAO.getOrders(userID);
        }

        public Task Reject(int id)
        {
            throw new NotImplementedException();
        }
    }
}
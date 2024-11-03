using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Models;
using Repository.Interfaces;

namespace BusinessLogic.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _IOrderRepository;
        public OrderServices(IOrderRepository orderRepository)
        {
            _IOrderRepository = orderRepository;
        }
        public async Task addOrder(Order order)
        {
            await _IOrderRepository.addOrder(order);
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            await _IOrderRepository.AddOrderDetail(orderDetail);
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
            return await _IOrderRepository.getOrder(orderId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _IOrderRepository.GetOrderDetails(orderId);
        }

        public async Task<IEnumerable<Order>> getOrders(int userID)
        {
            return await _IOrderRepository.getOrders(userID);
        }

        public Task Reject(int id)
        {
            throw new NotImplementedException();
        }
    }
}
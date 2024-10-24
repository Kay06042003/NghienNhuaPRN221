using BusinessLogic.Interfaces;
using Models;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _IOrderRepository;
        public OrderServices(IOrderRepository orderRepository) {
            _IOrderRepository = orderRepository;        
        }

        public async Task Approve(int id)
        {
            await _IOrderRepository.Approve(id);    
        }

        public async Task<Order> GetById(int id)
        {
            return await _IOrderRepository.GetById(id);
        }

        public async Task<IEnumerable<Order>> GetListOrderConfirm()
        {
            return await _IOrderRepository.GetListOrderConfirm();
        }

        public async Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id)
        {
            return await _IOrderRepository.GetListOrderDetail(id);
        }

        public async Task<IEnumerable<Order>> GetListOrderUpdate()
        {
            return await _IOrderRepository.GetListOrderUpdate();
        }

        public async Task Reject(int id)
        {
            await _IOrderRepository.Reject(id);
        }

        public async Task Update(Order item)
        {
            await _IOrderRepository.Update(item);
        }
    }
}

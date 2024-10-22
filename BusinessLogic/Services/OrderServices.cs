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

        public Task Approve(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetListOrderConfirm()
        {
            return await _IOrderRepository.GetListOrderConfirm();
        }

        public Task Reject(int id)
        {
            throw new NotImplementedException();
        }
    }
}

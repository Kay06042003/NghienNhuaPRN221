using BusinessLogic.Interfaces;
using Models;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<List<int>> GetMonthlyOrders()
        {
            return await _IOrderRepository.GetMonthlyOrders();
        }

        public async Task<int> GetOrdersInMonth()
        {
            return await _IOrderRepository.GetOrdersInMonth();
        }

        public async Task<int> GetPendingOrders()
        {
            return await _IOrderRepository.GetPendingOrders();
        }

        public Task<Dictionary<string, int>> GetSalesByCategory()
        {
            return _IOrderRepository.GetSalesByCategory();
        }

        public async Task<long> GetTotalRevenue()
        {
            return await _IOrderRepository.GetTotalRevenue();
        }

        public Task Reject(int id)
        {
            throw new NotImplementedException();
        }
    }
}

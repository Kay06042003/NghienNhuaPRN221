using DataAccess.DAO;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<List<int>> GetMonthlyOrders() => await _orderDAO.GetMonthlyOrders();

        public async Task<int> GetOrdersInMonth() => await _orderDAO.GetOrdersInMonth();

        public async Task<int> GetPendingOrders() => await _orderDAO.GetPendingOrders();

        public async Task<Dictionary<string, int>> GetSalesByCategory() => await _orderDAO.GetSalesByCategory();

        public async Task<long> GetTotalRevenue() => await _orderDAO.GetTotalRevenue();

        public Task Reject(int id)
        {
            throw new NotImplementedException();
        }
    }
}

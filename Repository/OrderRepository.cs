using DataAccess.DAO;
using Models;
using Repository.Interfaces;
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

        public async Task<List<int>> GetMonthlyOrders() => await _orderDAO.GetMonthlyOrders();

        public async Task<int> GetOrdersInMonth() => await _orderDAO.GetOrdersInMonth();

        public async Task<int> GetPendingOrders() => await _orderDAO.GetPendingOrders();

        public async Task<Dictionary<string, int>> GetSalesByCategory() => await _orderDAO.GetSalesByCategory();

        public async Task<long> GetTotalRevenue() => await _orderDAO.GetTotalRevenue();

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

        public async Task<IEnumerable<Order>> GetOrderStatisticDay(string date)
        {
            return await _orderDAO.GetOrderStatisticDay(date);
        }

        public async Task<IEnumerable<Order>> GetOrderStatisticMonth(string month)
        {
            return await _orderDAO.GetOrderStatisticMonth(month);
        }
        public async Task<IEnumerable<Order>> GetOrderStatisticYear(string year)
        {
            return await _orderDAO.GetOrderStatisticYear(year);
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


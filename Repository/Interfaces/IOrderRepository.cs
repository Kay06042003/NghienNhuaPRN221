using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DAO;
using Models;
using System.Collections.ObjectModel;
using System.Text;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetListOrderConfirm();
        Task Approve(int id);
        Task Reject(int id);
        Task addOrder(Order order);
        Task<Order> getOrder(int orderId);
        Task<IEnumerable<Order>> getOrders(int userID);
        Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId);
        Task AddOrderDetail(OrderDetail orderDetail);
        Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id);
        Task<Order> GetById(int id);
        Task<IEnumerable<Order>> GetListOrderUpdate();
        Task Update(Order item);
        Task<IEnumerable<Order>> GetOrderStatisticDay(string date);
        Task<IEnumerable<Order>> GetOrderStatisticMonth(string month);
        Task<IEnumerable<Order>> GetOrderStatisticYear(string year);
        Task<int> GetOrdersInMonth();
        Task<int> GetPendingOrders();
        Task<long> GetTotalRevenue();
        Task<List<int>> GetMonthlyOrders();
        Task<Dictionary<string, int>> GetSalesByCategory();
    }
}


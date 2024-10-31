using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IOrderServices
    {
        Task<IEnumerable<Order>> GetListOrderConfirm();
        Task Approve(int id);
        Task Reject(int id);
        Task<int> GetOrdersInMonth();
        Task<int> GetPendingOrders();
        Task<long> GetTotalRevenue();
        Task<List<int>> GetMonthlyOrders();
        Task<Dictionary<string, int>> GetSalesByCategory();

        Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id);
        Task<Order> GetById(int id);
        Task<IEnumerable<Order>> GetListOrderUpdate();
        Task Update(Order item);
        Task<IEnumerable<Order>> GetOrderStatisticDay(string date);
        Task<IEnumerable<Order>> GetOrderStatisticMonth(string month);
        Task<IEnumerable<Order>> GetOrderStatisticYear(string year);
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetListOrderConfirm();
        Task Approve(int id);
        Task Reject(int id);
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

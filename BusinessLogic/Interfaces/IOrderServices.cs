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
    }
}

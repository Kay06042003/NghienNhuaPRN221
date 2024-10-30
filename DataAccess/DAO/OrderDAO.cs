using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDAO : SingletonBase<OrderDAO>
    {
        public async Task<IEnumerable<Order>> GetListOrderConfirm()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<int> GetOrdersInMonth()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return await _context.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == currentMonth && o.OrderDate.Value.Year == currentYear)
                .CountAsync();
        }


        public async Task<int> GetPendingOrders()
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == "Pending")
                .CountAsync(); 
        }

        public async Task<long> GetTotalRevenue()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var orders = await _context.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == currentMonth && o.OrderDate.Value.Year == currentYear)
                .ToListAsync();

            long totalRevenue = 0;

            foreach (var order in orders)
            {
                if (long.TryParse(order.OrderTotalMoney, out long orderTotal))
                {
                    totalRevenue += orderTotal;
                }
            }
            return totalRevenue;
        }

        public async Task<List<int>> GetMonthlyOrders()
        {
            var currentYear = DateTime.Now.Year;
            var monthlyOrders = new List<int>(new int[12]);  // Khởi tạo danh sách 12 tháng, giá trị mặc định là 0

            // Truy vấn lấy số lượng đơn hàng trong từng tháng của năm hiện tại
            var orders = await _context.Orders
                .Where(o => o.OrderDate.Value.Year == currentYear)
                .GroupBy(o => o.OrderDate.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            // Cập nhật số lượng đơn hàng cho từng tháng
            foreach (var order in orders)
            {
                monthlyOrders[order.Month - 1] = order.Count;  // Tháng 1 tương ứng với chỉ mục 0
            }

            return monthlyOrders;
        }

        public async Task<Dictionary<string, int>> GetSalesByCategory()
        {
            var result = await (from od in _context.OrderDetails
                                join p in _context.Products on od.ProId equals p.ProId
                                group od by p.ProCategory into g
                                select new
                                {
                                    Category = g.Key,
                                    QuantitySold = g.Sum(od => od.OdQuantity ?? 0) // Thay thế null bằng 0
                                })
                               .ToDictionaryAsync(g => g.Category, g => g.QuantitySold);

            return result;
        }

    }
}

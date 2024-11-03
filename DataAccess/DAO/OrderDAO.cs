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
        // curd order
        public Task addOrder(Order order)
        {
            _context.Orders.Add(order);
            return _context.SaveChangesAsync();
        }

        public async Task<Order> getOrder(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> getOrders(int userID)
        {
            return await _context.Orders.Where(x => x.UserId == userID).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
        }
        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
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
                .Where(o => o.OrderStatus == "Waiting Accept - COD")
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

        public async Task<IEnumerable<Order>> GetListOrderConfirm()
        {
            return await _context.Orders.AsNoTracking().Where(o => o.OrderStatus == "Waiting Accept - COD").ToListAsync();
        }

        public async Task Approve(int id)
        {
            Order order = await GetById(id);
            order.OrderStatus = "Accepted - COD";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }
        public async Task<Order> GetById(int id)
        {
            var item = await _context.Orders.FirstOrDefaultAsync(c => c.OrderId == id);
            if (item == null) return null;
            return item;
        }
        public async Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id)
        {
            return await _context.OrderDetails.AsNoTracking().Where(o => o.OrderId == id).ToListAsync();
        }
        public async Task Reject(int id)
        {
            Order order = await GetById(id);
            order.OrderStatus = "Rejected - COD";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Order>> GetListOrderUpdate()
        {
            var order = await _context.Orders.AsNoTracking().Where(o => o.OrderStatus != "Waiting Accept - COD" && o.OrderStatus != "Rejected - COD"
            && o.OrderStatus != "Delivery successful - COD" && o.OrderStatus != "Delivery successful - Banking"
            && o.OrderStatus != "Delivery failed - COD" && o.OrderStatus != "Delivery failed - Banking").ToListAsync();
            return order;

        }
        public async Task<IEnumerable<Order>> GetOrderStatisticDay(string date)
        {

            var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate.ToString() == date && o.OrderStatus != "Waiting Accept - COD"
            && o.OrderStatus != "Delivery successful - COD" && o.OrderStatus != "Delivery failed - COD" && o.OrderStatus != "Delivery failed - Banking" &&
            o.OrderStatus != "Delivery successful - Banking")
            .ToListAsync();
            return orders;

        }
        public async Task<IEnumerable<Order>> GetOrderStatisticMonth(string month)
        {
            var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate.Value.Month == int.Parse(month) && o.OrderStatus != "Waiting Accept - COD"
            && o.OrderStatus != "Delivery successful - COD" && o.OrderStatus != "Delivery failed - COD" && o.OrderStatus != "Delivery failed - Banking" &&
            o.OrderStatus != "Delivery successful - Banking")
            .ToListAsync();
            return orders;

        }
        public async Task<IEnumerable<Order>> GetOrderStatisticYear(string year)
        {
            var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate.Value.Year == int.Parse(year) && o.OrderStatus != "Waiting Accept - COD"
            && o.OrderStatus != "Delivery successful - COD" && o.OrderStatus != "Delivery failed - COD" && o.OrderStatus != "Delivery failed - Banking" &&
            o.OrderStatus != "Delivery successful - Banking")
            .ToListAsync();
            return orders;

        }

        public async Task Update(Order item)
        {
            var existingItem = await GetById(item.OrderId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(item);
            }
            await _context.SaveChangesAsync();
        }

    }
}

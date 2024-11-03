using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

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





    }
}

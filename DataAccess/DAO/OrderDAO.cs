using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDAO : SingletonBase<OrderDAO>
    {
        public async Task<IEnumerable<Order>> GetListOrderConfirm()
        {
            return await _context.Orders.Where(o => o.OrderStatus=="Chờ Xác Nhận - COD").ToListAsync();
        }

        public async Task Approve(int id)
        {
            Order order =await GetById(id);
            order.OrderStatus = "Đã Xác Nhận - COD";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }
        public async Task<Order> GetById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id)
        {
            return await _context.OrderDetails.Where(o => o.OrderId == id).ToListAsync();
        }
        public async Task Reject(int id)
        {
            Order order = await GetById(id);
            order.OrderStatus = "Đã Từ Chối - COD";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Order>> GetListOrderUpdate()
        {
            return await _context.Orders.Where(o => o.OrderStatus!="Chờ Xác Nhận - COD" && o.OrderStatus!="Đã Từ Chối - COD" 
            && o.OrderStatus!="Giao Hàng Thành Công - COD" && o.OrderStatus != "Giao Hàng Thành Công - Banking" 
            && o.OrderStatus != "Giao Hàng Thất Bại - COD" && o.OrderStatus != "Giao Hàng Thất Bại - Banking").ToListAsync();
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

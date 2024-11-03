using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DAO;
using Models;

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
        
    }
}
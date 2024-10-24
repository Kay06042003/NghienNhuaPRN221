using Models;
using System;
using System.Collections.Generic;
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
        Task<IEnumerable<OrderDetail>> GetListOrderDetail(int id);
        Task<Order> GetById(int id);
        Task<IEnumerable<Order>> GetListOrderUpdate();
        Task Update(Order item);
    }
}

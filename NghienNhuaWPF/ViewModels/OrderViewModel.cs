using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        /*public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderName { get; set; }
        public string OrderTotalMoney { get; set; }
        public string OrderPhoneNumber { get; set; }
        public string OrderEmail { get; set; }
        public string OrderAddress { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }*/


        private readonly IOrderServices _orderServices;

        public ObservableCollection<Order> Orders { get; set; }

        public async Task LoadOrder()
        {
            var orders = await _orderServices.GetListOrderConfirm();
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }
        public OrderViewModel(IOrderServices orderServices)
        {
            Orders = new ObservableCollection<Order>();
            _orderServices = orderServices;
            _ = LoadOrder();
        }

    }

}

    using System;
using System.Collections.Generic;

#nullable disable

namespace Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderName { get; set; }
        public string OrderTotalMoney { get; set; }
        public string OrderPhoneNumber { get; set; }
        public string OrderEmail { get; set; }
        public string OrderAddress { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

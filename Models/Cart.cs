using System;
using System.Collections.Generic;

#nullable disable

namespace Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public int ProId { get; set; }
        public int? CartQuantity { get; set; }

        public virtual Product Pro { get; set; }
        public virtual User User { get; set; }
    }
}

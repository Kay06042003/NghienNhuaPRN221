using System;
using System.Collections.Generic;

#nullable disable

namespace Models
{
    public partial class Account
    {
        public int AccId { get; set; }
        public string AccGmail { get; set; }
        public string AccPassword { get; set; }
        public string Role { get; set; }

        public virtual User User { get; set; }
        public virtual staff staff { get; set; }
    }
}

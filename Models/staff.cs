using System;
using System.Collections.Generic;

#nullable disable

namespace Models
{
    public partial class staff
    {
        public int StaffId { get; set; }
        public string StaffSalary { get; set; }
        public string StaffGender { get; set; }
        public DateTime? SftaffDayOut { get; set; }
        public DateTime? StaffDayJoin { get; set; }
        public int? AccId { get; set; }
        public string StaffFullname { get; set; }
        public string StaffPhoneNumber { get; set; }
        public string StaffAddress { get; set; }
        public DateTime? StaffDateOfBirth { get; set; }
        public string StaffCitizenIdentityNumber { get; set; }
        public string StaffStatus { get; set; }

        public virtual Account Acc { get; set; }
    }
}

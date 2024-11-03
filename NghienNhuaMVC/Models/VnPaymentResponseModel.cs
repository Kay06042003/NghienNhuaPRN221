using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NghienNhuaMVC.Models
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }

    public class VnPayResqestModel
    {
        public int OrderId { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public double amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
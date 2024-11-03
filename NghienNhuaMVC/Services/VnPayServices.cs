using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NghienNhuaMVC.Models;

namespace NghienNhuaMVC.Services
{
    public class VnPayServices : IVnPayServices
    {
        private readonly IConfiguration _conf;
        public VnPayServices(IConfiguration configuration)
        {
            _conf = configuration;
        }
        public string CreateRequestUrl(HttpContext context, VnPayResqestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _conf["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _conf["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _conf["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _conf["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _conf["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "order"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _conf["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            var paymentURl = vnpay.CreateRequestUrl(_conf["VnPay:BaseUrl"], _conf["VnPay:HashSecret"]);
            return paymentURl;
        }
        public VnPaymentResponseModel PaymentExcute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(value) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var vnp_orderID = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collection.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _conf["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                    // VnPayResponseCode = "97",
                    // OrderDescription = "Lỗi xác thực dữ liệu",
                    // OrderId = vnp_orderID.ToString(),
                    // PaymentId = vnp_vnpayTranId.ToString(),
                    // TransactionId = vnp_vnpayTranId.ToString(),
                    // Token = vnp_vnpayTranId.ToString()
                };
            }
            else
            {
                return new VnPaymentResponseModel
                {
                    Success = true,
                    PaymentMethod = "VnPay",
                    VnPayResponseCode = vnp_ResponseCode,
                    OrderDescription = vnp_OrderInfo,
                    OrderId = vnp_orderID.ToString(),
                    PaymentId = vnp_vnpayTranId.ToString(),
                    TransactionId = vnp_vnpayTranId.ToString(),
                    Token = vnp_vnpayTranId.ToString()
                };
            }
        }
    }
}
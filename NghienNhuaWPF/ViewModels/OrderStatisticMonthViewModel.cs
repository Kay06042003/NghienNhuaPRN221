using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class OrderStatisticMonthViewModel : BaseViewModel
    {
        private readonly IOrderServices _orderServices;
        public ICommand GetStatisticMonthCommand { get; }
        public ICommand ExportCommand { get; }
        public ObservableCollection<Order> Orders { get; set; }

        private string _orderMonth;
        public string MonthStatistic
        {
            get { return _orderMonth; }
            set
            {
                _orderMonth = value;
                OnPropertyChanged(nameof(MonthStatistic));
            }
        }



        public OrderStatisticMonthViewModel(IOrderServices orderServices)
        {
            Orders = new ObservableCollection<Order>();
            _orderServices = orderServices;
            GetStatisticMonthCommand = new RelayCommand(param => GetOrderStatisticMonth((string)param), null);
            ExportCommand = new RelayCommand(ExportToExcel);
        }


        public async void GetOrderStatisticMonth(string date)
        {
            try
            {
                if (IsNumber(date) && int.Parse(date) > 0 && int.Parse(date) < 13)
                {
                    var orders = await _orderServices.GetOrderStatisticMonth(date);
                    Orders.Clear();

                    foreach (var order in orders)
                    {
                        Orders.Add(order);
                    }

                    if (Orders.Count() == 0)
                    {
                        MessageBox.Show("Không có đơn hàng nào được tìm thầy. ");
                    }
                }
                else if (date.Length == 0)
                {
                    MessageBox.Show("Vui lòng nhập tháng muốn hiển thị. ");
                }
                else
                {
                    MessageBox.Show("Giá trị bạn nhập không hợp lệ. ");
                    MonthStatistic = "";
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Giá trị bạn nhập không hợp lệ. " + ex.InnerException);
                MonthStatistic = "";
            }
        }
        private void ExportToExcel(object obj)
        {
            if (Orders.Count() == 0 && MonthStatistic.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tháng muốn xuất dữ liệu. ");
            }
            else if (MonthStatistic.Length != 0 && Orders.Count() == 0)
            {
                System.Windows.MessageBox.Show("Không tìm thấy đơn hàng nào. ");
            }
            else
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                string excelFolder = Path.Combine(projectRoot, "Excels");

                if (!Directory.Exists(excelFolder))
                {
                    Directory.CreateDirectory(excelFolder);
                }

                string filePath = Path.Combine(excelFolder, "OrderStatisticMonth.xlsx");

                try
                {
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Orders");

                        string[] headers = { "ID", "UserID", "OrderDate", "OrderStatus", "OrderName", "OrderTotalMoney", "OrderPhoneNumber", "OrderEmail", "OrderAddress" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = headers[i];
                        }

                        int rowIndex = 2;
                        foreach (var order in Orders)
                        {
                            worksheet.Cells[rowIndex, 1].Value = order.OrderId;
                            worksheet.Cells[rowIndex, 2].Value = order.UserId;
                            worksheet.Cells[rowIndex, 3].Value = order.OrderDate;
                            worksheet.Cells[rowIndex, 4].Value = order.OrderStatus;
                            worksheet.Cells[rowIndex, 5].Value = order.OrderName;
                            worksheet.Cells[rowIndex, 6].Value = order.OrderTotalMoney;
                            worksheet.Cells[rowIndex, 7].Value = order.OrderPhoneNumber;
                            worksheet.Cells[rowIndex, 8].Value = order.OrderEmail;
                            worksheet.Cells[rowIndex, 9].Value = order.OrderAddress;

                            rowIndex++;
                        }

                        package.SaveAs(new FileInfo(filePath));
                    }

                    MessageBox.Show($"Dữ liệu đã được xuất ra file Excel tại: {filePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}");
                }
            }
        }
        public OrderStatisticMonthViewModel()
        {

        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
        public bool IsNumber(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}

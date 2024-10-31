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
    public class OrderStatisticDayViewModel : BaseViewModel
    {
        private readonly IOrderServices _orderServices;
        public ICommand GetStatisticDayCommand { get; }
        public ICommand ExportCommand { get; }

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private string _orderDate;
        public string DayStatistic
        {
            get { return _orderDate; }
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(DayStatistic));
            }
        }



        public OrderStatisticDayViewModel(IOrderServices orderServices)
        {
            Orders = new ObservableCollection<Order>();
            _orderServices = orderServices;
            GetStatisticDayCommand = new RelayCommand(param => GetOrderStatisticDay((string)param), null);
            ExportCommand = new RelayCommand(ExportToExcel);

        }


        public async void GetOrderStatisticDay(string date)
        {
            try
            {
                date = date.Replace("/", "-");
                string[] parts = date.Split('-');
                if (parts[0].Length == 1)
                {
                    parts[0] = "0" + parts[0];
                }
                date = $"{parts[2]}-{parts[1]}-{parts[0]}";
                var orders = await _orderServices.GetOrderStatisticDay(date);
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
            catch (Exception ex)
            {
                MessageBox.Show("Vui lòng nhập ngày muốn hiển thị. " + ex.InnerException);
            }
        }

        private void ExportToExcel(object obj)
        {
            if (Orders.Count() == 0)
            {
                MessageBox.Show("Vui lòng nhập ngày muốn xuất dữ liệu. ");
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

                string filePath = Path.Combine(excelFolder, "OrderStatisticDay.xlsx");

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
        public OrderStatisticDayViewModel()
        {

        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}

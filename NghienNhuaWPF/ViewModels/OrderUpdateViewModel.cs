using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class OrderUpdateViewModel : BaseViewModel
    {
        private readonly IOrderServices _orderServices;
        public ObservableCollection<Order> OrderUpdates { get; set; }
        public ICommand UpdateCommand
        { get; }

        private int _orderId;
        public int OrderId
        {
            get { return _orderId; }
            set
            {
                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }
        private int? _userId;
        public int? UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
        private DateTime? _orderDate;
        public DateTime? OrderDate
        {
            get { return _orderDate; }
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }
        private string _orderStatus;
        public string OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                _orderStatus = value;
                OnPropertyChanged(nameof(OrderStatus));
            }
        }
        private string _orderStatusUpdate;
        public string OrderStatusUpdate
        {
            get { return _orderStatusUpdate; }
            set
            {
                _orderStatusUpdate = value;
                OnPropertyChanged(nameof(OrderStatusUpdate));
            }
        }
        private string _orderName;
        public string OrderName
        {
            get { return _orderName; }
            set
            {
                _orderName = value;
                OnPropertyChanged(nameof(OrderName));
            }
        }
        private string _orderTotalMoney;
        public string OrderTotalMoney
        {
            get { return _orderTotalMoney; }
            set
            {
                _orderTotalMoney = value;
                OnPropertyChanged(nameof(OrderTotalMoney));
            }
        }
        private string _orderPhoneNumber;
        public string OrderPhoneNumber
        {
            get { return _orderPhoneNumber; }
            set
            {
                _orderPhoneNumber = value;
                OnPropertyChanged(nameof(OrderPhoneNumber));
            }
        }
        private string _orderEmail;
        public string OrderEmail
        {
            get { return _orderEmail; }
            set
            {
                _orderEmail = value;
                OnPropertyChanged(nameof(OrderEmail));
            }
        }
        private string _orderAddress;
        public string OrderAddress
        {
            get { return _orderAddress; }
            set
            {
                _orderAddress = value;
                OnPropertyChanged(nameof(OrderAddress));
            }
        }

        public bool _isItemSelected;

        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set
            {
                _isItemSelected = value;
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        private Order _selectedOrder;

        public Order selectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(_selectedOrder));
                if (_selectedOrder != null)
                {
                    OrderId = _selectedOrder.OrderId;
                    OrderDate = _selectedOrder.OrderDate;
                    OrderStatus = _selectedOrder.OrderStatus;
                    OrderName = _selectedOrder.OrderName;
                    OrderAddress = _selectedOrder.OrderAddress;
                    OrderEmail = _selectedOrder.OrderEmail;
                    OrderPhoneNumber = _selectedOrder.OrderPhoneNumber;
                    OrderTotalMoney = _selectedOrder.OrderTotalMoney;
                    UserId = _selectedOrder.UserId;
                    IsItemSelected = true;
                }
                else
                {
                    IsItemSelected = false;
                }
            }
        }

        public async Task LoadOrderUpdate()
        {
            try
            {
                var orders = await _orderServices.GetListOrderUpdate();
                OrderUpdates.Clear();
                foreach (var order in orders)
                {
                    OrderUpdates.Add(order);
                }
            }
            catch (Exception e)
            {

            }

        }

        public OrderUpdateViewModel(IOrderServices orderServices)
        {
            OrderUpdates = new ObservableCollection<Order>();
            _orderServices = orderServices;
            _ = LoadOrderUpdate();
            UpdateCommand = new RelayCommand(UpdateOrder);



        }
        public OrderUpdateViewModel()
        {

        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
        public async void UpdateOrder(object parameter)
        {
            if (selectedOrder == null)
            {
                ShowErrorMessage("Vui lòng chọn đơn hàng cần cập nhật.");
                return;
            }

            try
            {
                string[] s = OrderStatus.Split("-");
                if (s[1].Length > 4)
                {
                    selectedOrder.OrderStatus += " - Banking";
                }
                else
                {
                    selectedOrder.OrderStatus += " - COD";
                }
                selectedOrder.OrderId = OrderId;
                selectedOrder.UserId = UserId;
                selectedOrder.OrderName = OrderName;
                selectedOrder.OrderAddress = OrderAddress;
                selectedOrder.OrderPhoneNumber = OrderPhoneNumber;
                selectedOrder.OrderEmail = OrderEmail;
                selectedOrder.OrderDate = OrderDate;
                OrderStatus = selectedOrder.OrderStatus;
                selectedOrder.OrderTotalMoney = OrderTotalMoney;
                await _orderServices.Update(selectedOrder);
                OrderStatusUpdate = "";

                ShowErrorMessage("Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Đã xảy ra lỗi.");
                throw new Exception("An error occurred while updating the order.", ex);
            }
            finally
            {
                await LoadOrderUpdate();
            }
        }
    }
}

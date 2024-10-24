using BusinessLogic.Interfaces;
using BusinessLogic.Services;
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
    public class OrderConfirmViewModel : BaseViewModel
    {
        private readonly IOrderServices _orderServices;
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<OrderDetail> OrderDetails { get; set; }
        public ICommand UpdateStatusOrder { get; private set; }

        private int _orderId;
        public int OrderId {
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
                OnPropertyChanged();
                if(_selectedOrder != null)
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
                    _=LoadOrderDetail(OrderId);
                }
                else
                {
                    IsItemSelected = false;
                }
            }
        }

        public async Task LoadOrderConfirm()
        {
            var orders = await _orderServices.GetListOrderConfirm();
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }

        public async Task LoadOrderDetail(int id)
        {
            var orderdetails = await _orderServices.GetListOrderDetail(id);
            OrderDetails.Clear();
            foreach (var orderdetail in orderdetails)
            {
                OrderDetails.Add(orderdetail);
            }
        }

       

        public ICommand ApproveCommand
        { get; }
        public ICommand RejectCommand
        { get; }
       
        public OrderConfirmViewModel(IOrderServices orderServices)
        {
            Orders = new ObservableCollection<Order>();
            OrderDetails = new ObservableCollection<OrderDetail>();
            _orderServices = orderServices;
            _ = LoadOrderConfirm();

            ApproveCommand = new RelayCommand(param => ApproveOrder((int)param), null);
            RejectCommand = new RelayCommand(param => RejectOrder((int)param), null);

        }
        public OrderConfirmViewModel()
        {

        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
       
        public async void ApproveOrder(int id)
        {
            if (MessageBox.Show("Confirm approve of this record?", "Student", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {

                    await _orderServices.Approve(id);
                    OrderDetails.Clear();
                    MessageBox.Show("Already Comfirm.");
                }
                catch (Exception ex)
                {
                    OrderDetails.Clear();
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    await LoadOrderConfirm();
                }
            }
        }

        public async void RejectOrder(int id)
        {
            if (MessageBox.Show("Confirm reject of this record?", "Student", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {

                    await _orderServices.Reject(id);
                    OrderDetails.Clear();
                    MessageBox.Show("Already Comfirm.");
                }
                catch (Exception ex)
                {
                    OrderDetails.Clear();
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    await LoadOrderConfirm();
                }
            }
        }

    }

}

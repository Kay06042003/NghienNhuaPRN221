using BusinessLogic.Interfaces;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NghienNhuaWPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IOrderServices _orderServices;

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection PieSeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public HomeViewModel(IOrderServices orderServices)
        {
            _orderServices = orderServices;
            _ = LoadData();
            Formatter = value => value.ToString("N0");
        }

        private int _totalOrders;
        public int TotalOrders
        {
            get { return _totalOrders; }
            set
            {
                _totalOrders = value;
                OnPropertyChanged(nameof(TotalOrders));
            }
        }

        private int _pendingOrders;
        public int PendingOrders
        {
            get { return _pendingOrders; }
            set
            {
                _pendingOrders = value;
                OnPropertyChanged(nameof(PendingOrders));
            }
        }

        private long _totalRevenue;
        public long TotalRevenue
        {
            get { return _totalRevenue; }
            set
            {
                _totalRevenue = value;
                OnPropertyChanged(nameof(TotalRevenue));
            }
        }

        public async Task LoadData()
        {
            TotalOrders = await _orderServices.GetOrdersInMonth();
            PendingOrders = await _orderServices.GetPendingOrders();
            TotalRevenue = await _orderServices.GetTotalRevenue();

            var monthlyOrders = await _orderServices.GetMonthlyOrders();

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            SeriesCollection = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Orders",
                Values = new ChartValues<int>(monthlyOrders)
            }
        };

            var salesByCategory = await _orderServices.GetSalesByCategory();

            PieSeriesCollection = new SeriesCollection();

            foreach (var category in salesByCategory)
            {
                PieSeriesCollection.Add(new PieSeries
                {
                    Title = category.Key,
                    Values = new ChartValues<int> { category.Value },
                    DataLabels = true
                });
            }

            OnPropertyChanged(nameof(PieSeriesCollection));
        }
    }
}

using NghienNhuaWPF.View;
using NghienNhuaWPF.ViewModels;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NghienNhuaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly StaffViewModel _staffViewModel;
        private readonly KeyboardViewModel _keyboardViewModel;
        private readonly KeycapViewModel _keycapViewModel;
        private readonly SwitchViewModel _switchViewModel;
        private readonly KitViewModel _kitViewModel;
        private readonly MouseViewModel _mouseViewModel;
        private readonly EarphoneViewModel _eyephoneViewModel;
        private readonly HomeViewModel _homeViewModel;
        private readonly OrderConfirmViewModel _orderConfirmViewModel;
        private readonly OrderUpdateViewModel _orderUpdateViewModel;
        private readonly OrderStatisticDayViewModel _orderStatisticDayViewModel;
        private readonly OrderStatisticMonthViewModel _orderStatisticMonthViewModel;
        private readonly OrderStatisticYearViewModel _orderStatisticYearViewModel;
        public MainView(MainViewModel mainViewModel, StaffViewModel staffViewModel, KeyboardViewModel keyboardViewModel,
            KeycapViewModel keycapViewModel, SwitchViewModel switchViewModel, KitViewModel kitViewModel,
            MouseViewModel mouseViewModel, HomeViewModel homeViewModel, EarphoneViewModel eyephoneViewModel, OrderConfirmViewModel orderConfirmViewModel,
            OrderUpdateViewModel orderUpdateViewModel, OrderStatisticDayViewModel orderStatisticDayViewModel, OrderStatisticMonthViewModel orderStatisticMonthViewModel,
            OrderStatisticYearViewModel orderStatisticYearViewModel)
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
            _staffViewModel = staffViewModel;
            _keyboardViewModel = keyboardViewModel;
            _keycapViewModel = keycapViewModel;
            _switchViewModel = switchViewModel;
            _kitViewModel = kitViewModel;
            _mouseViewModel = mouseViewModel;
            _homeViewModel = homeViewModel;
            _eyephoneViewModel = eyephoneViewModel;
            _orderConfirmViewModel = orderConfirmViewModel;
            _orderUpdateViewModel = orderUpdateViewModel;
            _orderStatisticDayViewModel = orderStatisticDayViewModel;
            _orderStatisticMonthViewModel = orderStatisticMonthViewModel;
            _orderStatisticYearViewModel = orderStatisticYearViewModel;


        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void HomeView_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = new HomeView(_homeViewModel);
        }

        private void StaffView_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = new StaffView(_staffViewModel);
        }

        private void KeyboardView_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = new KeyboardView(_keyboardViewModel);
        }

        private void KeycapView_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = new KeycapView(_keycapViewModel);
        }

        private void SwitchView_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = new SwitchView(_switchViewModel);
        }

        private void KitView_Click(Object sender, RoutedEventArgs e)
        {
            frMain.Content = new KitView(_kitViewModel);
        }

        private void MouseView_Click(object obj, RoutedEventArgs e)
        {
            frMain.Content = new MouseView(_mouseViewModel);
        }

        private void EarphoneView_Click(object obj, RoutedEventArgs e)
        {
            frMain.Content = new EarphoneView(_eyephoneViewModel);
        }
        private void OrderConfirm_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = new OrderConfirmView(_orderConfirmViewModel);
        }
        private void OrderUpdate_Click(object sender, RoutedEventArgs e)
        {

            frMain.Content = new OrderUpdateView(_orderUpdateViewModel);
        }
        private void OrderStatisticDay_Click(object sender, RoutedEventArgs e)
        {

            frMain.Content = new OrderStatisticDayView(_orderStatisticDayViewModel);
        }

        private void OrderStatisticMonth_Click(object sender, RoutedEventArgs e)
        {

            frMain.Content = new OrderStatisticMonthView(_orderStatisticMonthViewModel);
        }
        private void OrderStatisticYear_Click(object sender, RoutedEventArgs e)
        {

            frMain.Content = new OrderStatisticYearView(_orderStatisticYearViewModel);
        }
    }
}
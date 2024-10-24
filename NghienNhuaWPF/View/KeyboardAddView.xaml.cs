using NghienNhuaWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NghienNhuaWPF.View
{
<<<<<<<< HEAD:NghienNhuaWPF/View/GetListOrderConfirm.xaml.cs
    public partial class GetListOrderConfirm : Window
    {
        public GetListOrderConfirm(OrderConfirmViewModel orderViewModel)
========
    /// <summary>
    /// Interaction logic for KeyboardAddView.xaml
    /// </summary>
    public partial class KeyboardAddView : Window
    {
        public KeyboardAddView()
>>>>>>>> CRUD_Staff:NghienNhuaWPF/View/KeyboardAddView.xaml.cs
        {
            InitializeComponent();
            DataContext = orderViewModel;
        }
    }
}

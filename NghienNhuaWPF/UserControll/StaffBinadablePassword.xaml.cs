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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NghienNhuaWPF.UserControll
{
    /// <summary>
    /// Interaction logic for StaffBinadablePassword.xaml
    /// </summary>
    public partial class StaffBinadablePassword : UserControl
    {
        public StaffBinadablePassword()
        {
            InitializeComponent();
            txtPassword.PasswordChanged += OnPasswordChanged;
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(StaffBinadablePassword),
                new PropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.PasswordChanged -= OnPasswordChanged; // Ngắt sự kiện để tránh vòng lặp
            Password = txtPassword.Password; // Cập nhật giá trị Password
            txtPassword.PasswordChanged += OnPasswordChanged; // Kích hoạt lại sự kiện
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StaffBinadablePassword passwordBox && passwordBox.txtPassword.Password != (string)e.NewValue)
            {
                passwordBox.txtPassword.Password = (string)e.NewValue; // Cập nhật PasswordBox khi thuộc tính Password thay đổi
            }
        }
    }
}

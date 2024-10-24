using BusinessLogic.Interfaces;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountServices _accountServices;
        public ICommand LoginCommand { get; }

        public LoginViewModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
            LoginCommand = new RelayCommand(Login);
        }

        private string _accGmail;
        public string accGmail
        {
            get
            {
                return _accGmail;
            }

            set
            {
                _accGmail = value;
                OnPropertyChanged(nameof(accGmail));
            }
        }
        private string _passWord;
        public string passWord
        {
            get
            {
                return _passWord;
            }

            set
            {
                _passWord = value;
                OnPropertyChanged(nameof(passWord));
            }
        }

        private bool _isViewVisible = true;
        public bool isViewVisible
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(isViewVisible));
            }
        }

        public string _errorMessage;
        public string errorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(errorMessage));
            }
        }

        private void Login(object parametter)
        {
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (string.IsNullOrEmpty(accGmail))
                {
                    MessageBox.Show("Tên đăng nhập không được để trống.");
                    return;
                }

                if (passWord == null || passWord.Length == 0)
                {
                    MessageBox.Show("Mật khẩu không được để trống.");
                    return;
                }

                // Chuyển đổi SecureString sang string và hash mật khẩu bằng MD5
                string passwordString = _accountServices.MD5Hash(passWord);
                // Gọi dịch vụ đăng nhập và kiểm tra kết quả
                var loginResult =  _accountServices.loginAccount(accGmail, passwordString);

                // Xử lý kết quả đăng nhập
                if (loginResult != null)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(accGmail), null);
                    
                    isViewVisible = false;
                    // Thực hiện các bước tiếp theo sau khi đăng nhập thành công (ví dụ: điều hướng tới màn hình chính)
                }
                else
                {
                    errorMessage = "* Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi có thể xảy ra trong quá trình đăng nhập
                MessageBox.Show($"Đã xảy ra lỗi khi đăng nhập: {ex.Message}");
            }
        }
        
    }
}

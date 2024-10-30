using BusinessLogic.Interfaces;
using Models;
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
        private readonly IStaffServices _staffServices;
        public ICommand LoginCommand { get; }

        public LoginViewModel(IAccountServices accountServices, IStaffServices staffServices)
        {
            _accountServices = accountServices;
            _staffServices = staffServices;
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

        private async void Login(object parametter)
        {
            try
            {
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

                string passwordString = _accountServices.MD5Hash(passWord);
                var loginResult = _accountServices.loginAccount(accGmail, passwordString);

                if (loginResult != null)
                { 
                    var account = await _accountServices.GetAccountByAccGmail(accGmail);

                    if (account.Role == "3" || account.Role == "2")
                    {
                        var user = await _staffServices.GetByAccId(account.AccId);
                        if (user.StaffStatus == "Working")
                        {
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(accGmail), null);
                            isViewVisible = false;
                        }
                        else 
                        {
                            MessageBox.Show("Tài khoản của dã bị khóa");
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Tài khoản của bạn không có quyền truy cập vào ứng dụng này");
                    }
                    
                }
                else
                {
                    errorMessage = "* Sai Email hoặc mật khẩu";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi đăng nhập: {ex.Message}");
            }
        }

    }
}

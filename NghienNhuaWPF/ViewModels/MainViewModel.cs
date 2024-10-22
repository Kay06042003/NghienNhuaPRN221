using BusinessLogic.Interfaces;
using FontAwesome.Sharp;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private User _currentUser;
        private string _caption;
        private IconChar _icon;

        private IUserServices _userServices;
        private IAccountServices _accountServices;

        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }


        public MainViewModel(IAccountServices accountServices, IUserServices userServices)
        {
            _accountServices = accountServices;
            _userServices = userServices;
            CurrentUser = new User();
            LoadCurrentUserData();
        }

        private async void LoadCurrentUserData()
        {
            var account = await _accountServices.GetAccountByAccGmail(Thread.CurrentPrincipal.Identity.Name);
            if (account != null)
            {
                var user = await _userServices.GetUserByAccId(account.AccId);

                CurrentUser = new User
                {
                    UserId = user.UserId,
                    UserFullname = user.UserFullname,
                    UserSdt = user.UserSdt,
                    UserAddress = user.UserAddress
                };
            }
        }
    }
}

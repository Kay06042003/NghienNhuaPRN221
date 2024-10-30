using BusinessLogic.Interfaces;
using FontAwesome.Sharp;
using Models;

namespace NghienNhuaWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Staff _currentUser;
        private string _caption;
        private IconChar _icon;

        private IStaffServices _staffServices;
        private IAccountServices _accountServices;

        public Staff CurrentStaff
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentStaff));
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


        public MainViewModel(IAccountServices accountServices, IStaffServices staffServices)
        {
            _accountServices = accountServices;
            CurrentStaff = new Staff();
            LoadCurrentUserData();
            _staffServices = staffServices;
        }

        private async void LoadCurrentUserData()
        {
            var account = await _accountServices.GetAccountByAccGmail(Thread.CurrentPrincipal.Identity.Name);
            if (account != null)
            {
                var user = await _staffServices.GetByAccId(account.AccId);

                CurrentStaff = new Staff
                    {
                        StaffId = user.StaffId,
                        StaffFullname = user.StaffFullname,
                        StaffAddress = user.StaffAddress,
                        StaffCitizenIdentityNumber = user.StaffCitizenIdentityNumber,
                        StaffDateOfBirth = user.StaffDateOfBirth,
                        StaffStatus = user.StaffStatus,
                    };
            }
        }
    }
}

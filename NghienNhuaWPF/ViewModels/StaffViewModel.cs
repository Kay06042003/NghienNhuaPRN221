using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly IStaffServices _staffService;
        private readonly IAccountServices _accountServices;

        public ObservableCollection<Staff> Staffs { get; set; }

        public ICommand AddStaffCommand { get; }
        public ICommand RemoveStaffCommand { get; }
        public ICommand UpdateStaffCommand { get; }
        public ICommand CancelCommand { get; }

        public ICommand DeleteCommand
        { get; }

        public ICommand SelectCommand
        { get; }

        public StaffViewModel(IStaffServices staffService, IAccountServices accountServices)
        {
            Staffs = new ObservableCollection<Staff>();
            _accountServices = accountServices;
            _staffService = staffService;
            _ = LoadStaffs();
            AddStaffCommand = new RelayCommand(AddStaff);
            UpdateStaffCommand = new RelayCommand(UpdateStaff);
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(param => DeleteStaff((int)param), null);
            SelectCommand = new RelayCommand(param => selectStaff((int)param),null);
        }

        private string _staffGmail;
        public string staffGmail
        {
            get { return _staffGmail; }
            set
            {
                _staffGmail = value;
                OnPropertyChanged(nameof(staffGmail));
            }
        }

        private string _staffPassword;
        public string staffPassword
        {
            get { return _staffPassword; }
            set
            {
                _staffPassword = value;
                OnPropertyChanged(nameof(staffPassword));
            }
        }

        private int _staffId;
        public int staffId
        {
            get { return _staffId; }
            set
            {
                _staffId = value;
                OnPropertyChanged(nameof(staffId));
            }
        }

        private string _staffSalary;
        public string staffSalary
        {
            get { return _staffSalary; }
            set
            {
                _staffSalary = value;
                OnPropertyChanged(nameof(staffSalary));
            }
        }

        private string _staffGender;
        public string staffGender
        {
            get { return _staffGender; }
            set
            {
                _staffGender = value;
                OnPropertyChanged(nameof(staffGender));
            }
        }

        private DateTime? _staffDateOfBirth;
        public DateTime? staffDateOfBirth
        {
            get { return _staffDateOfBirth; }
            set
            {
                _staffDateOfBirth = value;
                OnPropertyChanged(nameof(staffDateOfBirth));
            }
        }

        private string _staffFullname;
        public string staffFullname
        {
            get { return _staffFullname; }
            set
            {
                _staffFullname = value;
                OnPropertyChanged(nameof(staffFullname));
            }
        }

        private string _staffPhoneNumber;
        public string staffPhoneNumber
        {
            get { return _staffPhoneNumber; }
            set
            {
                _staffPhoneNumber = value;
                OnPropertyChanged(nameof(staffPhoneNumber));
            }
        }

        private string _staffAddress;
        public string staffAddress
        {
            get { return _staffAddress; }
            set
            {
                _staffAddress = value;
                OnPropertyChanged(nameof(staffAddress));
            }
        }

        private string _staffCitizenIdentityNumber;
        public string staffCitizenIdentityNumber
        {
            get { return _staffCitizenIdentityNumber; }
            set
            {
                _staffCitizenIdentityNumber = value;
                OnPropertyChanged(nameof(staffCitizenIdentityNumber));
            }
        }

        private bool _isItemSelected;
        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set
            {
                _isItemSelected = value;
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        private Staff _selectedStaff;
        public Staff selectedStaff
        {
            get { return _selectedStaff; }
            set
            {
                _selectedStaff = value;
                OnPropertyChanged(nameof(_selectedStaff));
            }
        }

        private async void selectStaff(int id) 
        {
            _selectedStaff = await _staffService.GetByIdStaff(id);
            if (_selectedStaff != null)
            {
                staffId = _selectedStaff.StaffId;
                staffFullname = _selectedStaff.StaffFullname;
                staffSalary = _selectedStaff.StaffSalary;
                staffGender = _selectedStaff.StaffGender;
                staffDateOfBirth = _selectedStaff.StaffDateOfBirth;
                staffPhoneNumber = _selectedStaff.StaffPhoneNumber;
                staffAddress = _selectedStaff.StaffAddress;
                staffCitizenIdentityNumber = _selectedStaff.StaffCitizenIdentityNumber;
                IsItemSelected = true;
                staffGmail = "";
            }
        }

        public async Task LoadStaffs()
        {
            var staffs = await _staffService.GetListAllStaff();
            Staffs.Clear();
            foreach (var staff in staffs)
            {
                Staffs.Add(staff);
            }
        }

        private void Cancel(object parameter)
        {
            selectedStaff = null;
            ResetStaffData(); // Optional: Resets all input fields if required
        }

        private async void AddStaff(object parameter)
        {
            int createdAccountId = 0;
            string hashPassword = _accountServices.MD5Hash(staffPassword);

            if (string.IsNullOrEmpty(staffGmail) ||
                   string.IsNullOrEmpty(staffPassword) || IsStaffDataInvalid() )
            {
                ShowErrorMessage("Các trường thông tin không được để trống.");
                return;
            }

            Account existingAccount = await _accountServices.GetAccountByAccGmail(staffGmail);
            if (existingAccount != null)
            {
                ShowErrorMessage("Gmail đã tồn tại.");
                ResetStaffData();
                return;
            }

            try
            {
                var account = new Account
                {
                    AccGmail = staffGmail,
                    AccPassword = hashPassword,
                    Role = "2",
                };

                await _accountServices.Add(account);
                createdAccountId = account.AccId;

                var staff = new Staff
                {
                    StaffFullname = staffFullname,
                    StaffSalary = staffSalary,
                    StaffGender = staffGender,
                    StaffDateOfBirth = staffDateOfBirth,
                    StaffPhoneNumber = staffPhoneNumber,
                    StaffAddress = staffAddress,
                    StaffCitizenIdentityNumber = staffCitizenIdentityNumber,
                    AccId = createdAccountId,
                    StaffDayJoin = DateTime.Now,
                    StaffStatus = "Working"
                };

                await _staffService.AddStaff(staff);
                Staffs.Add(staff);

                ResetStaffData();
            }
            catch (Exception ex)
            {
                if (createdAccountId != 0)
                {
                    await _accountServices.Delete(createdAccountId);
                }
                ShowErrorMessage("Đã xảy ra lỗi khi thêm nhân viên.");
                throw new Exception("An error occurred while adding the staff.", ex);
            }
            
        }

        private async void UpdateStaff(object parameter)
        {
            if (selectedStaff == null || IsStaffDataInvalid())
            {
                ShowErrorMessage("Vui lòng chọn nhân viên cần cập nhật và điền đầy đủ thông tin.");
                return;
            }

            try
            {
                selectedStaff.StaffFullname = staffFullname;
                selectedStaff.StaffSalary = staffSalary;
                selectedStaff.StaffGender = staffGender;
                selectedStaff.StaffDateOfBirth = staffDateOfBirth;
                selectedStaff.StaffPhoneNumber = staffPhoneNumber;
                selectedStaff.StaffAddress = staffAddress;
                selectedStaff.StaffCitizenIdentityNumber = staffCitizenIdentityNumber;

                await _staffService.UpdateStaff(selectedStaff);


                ShowErrorMessage("Cập nhật thành công.");
                ResetStaffData();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Đã xảy ra lỗi.");
                throw new Exception("An error occurred while updating the staff.", ex);
            }
            finally
            {
                await LoadStaffs();
            }
        }

        public async void DeleteStaff(int id)
        {
            if (MessageBox.Show("Confirm delete of this record?", "Student", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    
                    await _staffService.DeleteStaff(id);
                    ResetStaffData();
                    MessageBox.Show("Record successfully deleted.");
                }
                catch (Exception ex)
                {
                    ResetStaffData();
                    MessageBox.Show("Error occured while saving. " + ex.InnerException);
                }
                finally
                {
                    await LoadStaffs();
                }
            }
        }

        private bool IsStaffDataInvalid()
        {
            return string.IsNullOrEmpty(staffFullname) ||
                   string.IsNullOrEmpty(staffSalary) ||
                   string.IsNullOrEmpty(staffGender) ||
                   staffDateOfBirth == null ||
                   string.IsNullOrEmpty(staffPhoneNumber) ||
                   string.IsNullOrEmpty(staffAddress) ||
                   string.IsNullOrEmpty(staffCitizenIdentityNumber);
        }


        private void ResetStaffData()
        {
            staffId = 0;
            staffGmail = staffPassword = staffFullname = staffSalary = staffGender = staffCitizenIdentityNumber = staffPhoneNumber = staffAddress = "";
            staffDateOfBirth = null;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}

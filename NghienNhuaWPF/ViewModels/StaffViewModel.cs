using BusinessLogic.Interfaces;
using Microsoft.VisualBasic.Devices;
using Models;
using NghienNhuaWPF.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly IStaffServices _staffService;
        private readonly IAccountServices _accountServices;

        public ObservableCollection<Staff> Staffs { get; set; }

        private string _accGmail;
        public string accGmail
        {
            get { return _accGmail; }
            set
            {
                _accGmail = value;
                OnPropertyChanged(nameof(accGmail));
            }
        }

        public ICommand AddStaffCommand { get; }
        public ICommand RemoveStaffCommand { get; }
        public ICommand UpdateStaffCommand { get; }
        public ICommand CancelCommand { get; }

        public ICommand DeleteCommand
        { get; }
        public ICommand RecoverCommand
        { get; }

        public ICommand SelectCommand
        { get; }

        public ICollectionView FilteredStaffs { get; set; }
        public ICommand ExportCommand { get; set; }


        public StaffViewModel(IStaffServices staffService, IAccountServices accountServices)
        {
            Staffs = new ObservableCollection<Staff>();
            _accountServices = accountServices;
            _staffService = staffService;

            accGmail = Thread.CurrentPrincipal.Identity.Name;
            FilteredStaffs = CollectionViewSource.GetDefaultView(Staffs);
            FilteredStaffs.Filter = FilterKeyboards;

            _ = LoadStaffs();


            AddStaffCommand = new RelayCommand(AddStaff);
            UpdateStaffCommand = new RelayCommand(UpdateStaff);
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(param => DeleteStaff((int)param), null);
            RecoverCommand = new RelayCommand(param => RecoverStaff((int)param), null);
            SelectCommand = new RelayCommand(param => selectStaff((int)param),null);
            ExportCommand = new RelayCommand(ExportToExcel);
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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilter();
            }
        }

        private bool FilterKeyboards(object item)
        {
            if (item is Staff staff)
            {
                // Điều chỉnh bộ lọc để tìm kiếm theo các trường bạn cần
                return string.IsNullOrEmpty(SearchText) ||
                       staff.StaffFullname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       staff.StaffGender.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       staff.StaffSalary.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       staff.StaffPhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       staff.StaffAddress.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       staff.StaffStatus.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredStaffs.Refresh();
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
            ResetStaffData();
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
            var currentStaff = await _staffService.GetByAccGmail(accGmail);
              
            var staff = await _staffService.GetByIdStaff(id);
            if (currentStaff.StaffId == staff.StaffId) 
            {
                MessageBox.Show("Bạn không thể xóa tài khoản của bạn");
                return;
            }
                if (staff.StaffStatus == "Working")
            {
                if (MessageBox.Show("Bạn chắc chắn muốn xóa nhân viên này?", "Student", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _staffService.DeleteStaff(id);
                        ResetStaffData();
                        MessageBox.Show("Xóa nhân viên thành công.");
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
            else 
            {
                MessageBox.Show("Nhân viên này đã nghỉ việc");
            }
        }

        public async void RecoverStaff(int id)
        {
            var staff = await _staffService.GetByIdStaff(id);
            if (staff.StaffStatus == "Tired")
            {
                if (MessageBox.Show("Bạn có muốn khôi phục tài khoản?", "Staff", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes)
                {
                    try
                    {

                        await _staffService.RecoverStaff(id);
                        ResetStaffData();
                        MessageBox.Show("Khôi phục tài khoản thành  công.");
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
            else
            {
                MessageBox.Show("Nhân viên này vẫn đang làm việc");
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

        private void ExportToExcel(object obj)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            string excelFolder = @"C:\Users\thinh\Documents\GitHub\NghienNhuaPRN221\NghienNhuaWPF\Excels";

            if (!Directory.Exists(excelFolder))
            {
                Directory.CreateDirectory(excelFolder);
            }

            string filePath = Path.Combine(excelFolder, "StaffsData.xlsx");

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Staff");

                    string[] headers = { "ID", "Full  Name", "Gender", "Phone Number", "Address", "Citizen Identity Number", "Date Of Birth", "Salary", "Day Join", "Day Out", "Status"};
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int rowIndex = 2;
                    foreach (var staff in Staffs)
                    {
                        worksheet.Cells[rowIndex, 1].Value = staff.StaffId;
                        worksheet.Cells[rowIndex, 2].Value = staff.StaffFullname;
                        worksheet.Cells[rowIndex, 3].Value = staff.StaffGender;
                        worksheet.Cells[rowIndex, 4].Value = staff.StaffPhoneNumber;
                        worksheet.Cells[rowIndex, 5].Value = staff.StaffAddress;
                        worksheet.Cells[rowIndex, 6].Value = staff.StaffCitizenIdentityNumber;
                        worksheet.Cells[rowIndex, 7].Value = staff.StaffDateOfBirth.ToString();
                        worksheet.Cells[rowIndex, 8].Value = staff.StaffSalary; 
                        worksheet.Cells[rowIndex, 9].Value = staff.StaffDayJoin.ToString();
                        worksheet.Cells[rowIndex, 10].Value = staff.SftaffDayOut.ToString();
                        worksheet.Cells[rowIndex, 11].Value = staff.StaffStatus;

                        rowIndex++;
                    }

                    package.SaveAs(new FileInfo(filePath));
                }

                MessageBox.Show($"Dữ liệu đã được xuất ra file Excel tại: {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}");
            }
        }
    }
}

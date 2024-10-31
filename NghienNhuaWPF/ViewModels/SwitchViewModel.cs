using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Microsoft.VisualBasic.Devices;
using Models;
using NghienNhuaWPF.Utilities;
using NghienNhuaWPF.View;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class SwitchViewModel : BaseViewModel
    {
        private readonly ISwitchServices _switchServices;
        private readonly IProductService _productService;
        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ObservableCollection<Switch> Switches { get; set; }
        public ICollectionView FilteredSwitchs { get; set; }

        public SwitchViewModel(IProductService productService, ISwitchServices switchServices)
        {
            Switches = new ObservableCollection<Switch>();
            _switchServices = switchServices;
            _productService = productService;
            FilteredSwitchs = CollectionViewSource.GetDefaultView(Switches);
            FilteredSwitchs.Filter = FilteredSwitch;

            _ = LoadSwitchs();

            ShowDetailCommand = new RelayCommand<Switch>(ShowDetail);
            AddNewCommand = new RelayCommand(ShowAddNew);
            ExportCommand = new RelayCommand(ExportToExcel);
            _switchServices = switchServices;
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

        private bool FilteredSwitch(object item)
        {
            if (item is Switch switchs)
            {
                // Điều chỉnh bộ lọc để tìm kiếm theo các trường bạn cần
                return string.IsNullOrEmpty(SearchText) ||
                       switchs.Pro.ProName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       switchs.Pro.ProPrice.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       switchs.Pro.ProBrand.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       switchs.Pro.ProOrigin.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       (switchs.Pro.ProQuantity?.ToString().Contains(SearchText) ?? false);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredSwitchs.Refresh();
        }
        private async void ShowAddNew(object obj)
        {
            var addSwitchView = new SwitchAddView();
            addSwitchView.DataContext = new SwitchAddViewModel(_switchServices, _productService);
            addSwitchView.ShowDialog();
            await LoadSwitchs();
        }

        private async void ShowDetail(Switch selectedSwitch)
        {
            if (selectedSwitch == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");
                return;
            }

            var detailWindow = new SwitchDetailView
            {
                DataContext = new SwitchDetailViewModel(_switchServices, selectedSwitch)
            };

            detailWindow.ShowDialog();
            await LoadSwitchs();
        }

        public async Task LoadSwitchs()
        {
            var switches = await _switchServices.GetListAll();
            Switches.Clear();
            foreach (var sw in switches)
            {
                Switches.Add(sw);
            }
        }

        private void ExportToExcel(object obj)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string excelFolder = Path.Combine(projectRoot, "Excels");

            if (!Directory.Exists(excelFolder))
            {
                Directory.CreateDirectory(excelFolder);
            }

            string filePath = Path.Combine(excelFolder, "SwitchesData.xlsx");

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Switches");

                    string[] headers = { "ID", "Name", "Quantity", "Price", "Description", "Discount", "Date", "Category", "Brand", "Origin", "Pin", "Type", "Srping", "Reliability", "Depth" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int rowIndex = 2;
                    foreach (var sw in Switches)
                    {
                        worksheet.Cells[rowIndex, 1].Value = sw.Pro.ProId;
                        worksheet.Cells[rowIndex, 2].Value = sw.Pro.ProName;
                        worksheet.Cells[rowIndex, 3].Value = sw.Pro.ProQuantity;
                        worksheet.Cells[rowIndex, 4].Value = sw.Pro.ProPrice;
                        worksheet.Cells[rowIndex, 5].Value = sw.Pro.ProDescription;
                        worksheet.Cells[rowIndex, 6].Value = sw.Pro.ProDiscount;
                        worksheet.Cells[rowIndex, 7].Value = sw.Pro.ProDate.ToString();
                        worksheet.Cells[rowIndex, 8].Value = sw.Pro.ProCategory;
                        worksheet.Cells[rowIndex, 9].Value = sw.Pro.ProBrand;
                        worksheet.Cells[rowIndex, 10].Value = sw.Pro.ProOrigin;
                        worksheet.Cells[rowIndex, 11].Value = sw.SwitchPin;
                        worksheet.Cells[rowIndex, 12].Value = sw.SwitchType;
                        worksheet.Cells[rowIndex, 13].Value = sw.SwitchSpring;
                        worksheet.Cells[rowIndex, 14].Value = sw.SwitchReliability;
                        worksheet.Cells[rowIndex, 15].Value = sw.SwitchDepth;

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

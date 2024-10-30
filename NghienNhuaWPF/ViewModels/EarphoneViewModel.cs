using BusinessLogic.Interfaces;
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
    public class EarphoneViewModel : BaseViewModel
    {
        private readonly IEarphoneServices _earphoneServices;
        private readonly IProductService _productService;
        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public ICollectionView FilteredEarPhones { get; set; }

        public ObservableCollection<Earphone> Earphones { get; set; }

        public EarphoneViewModel(IProductService productService, IEarphoneServices earphoneServices)
        {
            Earphones = new ObservableCollection<Earphone>();
            _earphoneServices = earphoneServices;
            _productService = productService;
            FilteredEarPhones = CollectionViewSource.GetDefaultView(Earphones);
            FilteredEarPhones.Filter = FilterEarphone;

            _ = LoadEarphones();

            ShowDetailCommand = new RelayCommand<Earphone>(ShowDetail);
            AddNewCommand = new RelayCommand(ShowAddNew);
            ExportCommand = new RelayCommand(ExportToExcel);
            _earphoneServices = earphoneServices;
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

        private bool FilterEarphone(object item)
        {
            if (item is Earphone earphone)
            {
                // Điều chỉnh bộ lọc để tìm kiếm theo các trường bạn cần
                return string.IsNullOrEmpty(SearchText) ||
                       earphone.Pro.ProName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       earphone.Pro.ProPrice.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       earphone.Pro.ProBrand.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       earphone.Pro.ProOrigin.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       (earphone.Pro.ProQuantity?.ToString().Contains(SearchText) ?? false);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredEarPhones.Refresh();
        }
        private async void ShowAddNew(object obj)
        {
            var addKeyboardView = new EarphoneAddView();
            addKeyboardView.DataContext = new EarphoneAddViewModel(_productService, _earphoneServices);
            addKeyboardView.ShowDialog();
            await LoadEarphones();
        }

        private async void ShowDetail(Earphone selectedEarphone)
        {
            if (selectedEarphone == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");
                return;
            }
            var detailWindow = new EarphoneDetailView
            {
                DataContext = new EarphoneDetailViewModel(selectedEarphone, _earphoneServices)
            };

            detailWindow.ShowDialog();
            await LoadEarphones();
        }

        public async Task LoadEarphones()
        {
            var earphones = await _earphoneServices.GetListAll();
            Earphones.Clear();
            foreach (var ear in earphones)
            {
                Earphones.Add(ear);
            }
        }

        private void ExportToExcel(object obj)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            string excelFolder = @"C:\Users\thinh\Documents\GitHub\NghienNhuaPRN221\NghienNhuaWPF\Excels";

            if (!Directory.Exists(excelFolder))
            {
                Directory.CreateDirectory(excelFolder);
            }

            string filePath = Path.Combine(excelFolder, "EarphonesData.xlsx");

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Earphones");

                    string[] headers = { "ID", "Name", "Quantity", "Price", "Description", "Discount", "Date", "Category", "Brand", "Origin", "Type", "Plug", "Compatibility", "Wire Length", "Utility", "Connect", "Control", "Charging Port", "EarConnectTech" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int rowIndex = 2;
                    foreach (var earphone in Earphones)
                    {
                        worksheet.Cells[rowIndex, 1].Value = earphone.Pro.ProId;
                        worksheet.Cells[rowIndex, 2].Value = earphone.Pro.ProName;
                        worksheet.Cells[rowIndex, 3].Value = earphone.Pro.ProQuantity;
                        worksheet.Cells[rowIndex, 4].Value = earphone.Pro.ProPrice;
                        worksheet.Cells[rowIndex, 5].Value = earphone.Pro.ProDescription;
                        worksheet.Cells[rowIndex, 6].Value = earphone.Pro.ProDiscount;
                        worksheet.Cells[rowIndex, 7].Value = earphone.Pro.ProDate.ToString();
                        worksheet.Cells[rowIndex, 8].Value = earphone.Pro.ProCategory;
                        worksheet.Cells[rowIndex, 9].Value = earphone.Pro.ProBrand;
                        worksheet.Cells[rowIndex, 10].Value = earphone.Pro.ProOrigin;
                        worksheet.Cells[rowIndex, 11].Value = earphone.EarType;
                        worksheet.Cells[rowIndex, 12].Value = earphone.EarPlug;
                        worksheet.Cells[rowIndex, 13].Value = earphone.EarCompatibility;
                        worksheet.Cells[rowIndex, 14].Value = earphone.EarWireLength;
                        worksheet.Cells[rowIndex, 15].Value = earphone.EarUtility;
                        worksheet.Cells[rowIndex, 16].Value = earphone.EarConnect;
                        worksheet.Cells[rowIndex, 17].Value = earphone.EarControl;
                        worksheet.Cells[rowIndex, 18].Value = earphone.EarChargingPort;
                        worksheet.Cells[rowIndex, 19].Value = earphone.EarConnectTech;

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

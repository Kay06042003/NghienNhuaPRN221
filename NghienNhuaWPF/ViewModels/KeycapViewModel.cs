using BusinessLogic.Interfaces;
using Microsoft.VisualBasic.Devices;
using Models;
using NghienNhuaWPF.Utilities;
using NghienNhuaWPF.View;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class KeycapViewModel : BaseViewModel
    {
        private readonly IKeycapServices _keycapServices;
        private readonly IProductService _productService;

        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ExportCommand { get; set; }


        public ObservableCollection<Keycap> Keycaps { get; set; }

        public ICollectionView FilteredKeycaps { get; set; }

        public KeycapViewModel(IKeycapServices keycapServices, IProductService productService)
        {
            Keycaps = new ObservableCollection<Keycap>();
            _keycapServices = keycapServices;
            _productService = productService;

            FilteredKeycaps = CollectionViewSource.GetDefaultView(Keycaps);
            FilteredKeycaps.Filter = FilterKeycaps;

            _ = LoadKeycaps();

            AddNewCommand = new RelayCommand(ShowAddNew);
            ShowDetailCommand = new RelayCommand<Keycap>(ShowDetail);
            ExportCommand = new RelayCommand(ExportToExcel);
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

        private bool FilterKeycaps(object item)
        {
            if (item is Keycap keycap)
            {
                // Điều chỉnh bộ lọc để tìm kiếm theo các trường bạn cần
                return string.IsNullOrEmpty(SearchText) ||
                       keycap.Pro.ProName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       keycap.Pro.ProPrice.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       keycap.Pro.ProBrand.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       keycap.Pro.ProOrigin.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       (keycap.Pro.ProQuantity?.ToString().Contains(SearchText) ?? false);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredKeycaps.Refresh();
        }
        private void ShowAddNew(object obj)
        {
            var addKeycapView = new KeycapAddView();
            addKeycapView.DataContext = new KeycapAddViewModel(_keycapServices, _productService);
            addKeycapView.ShowDialog();
            _ = LoadKeycaps();
        }

        private void ShowDetail(Keycap selectedKeycap)
        {
            if (selectedKeycap == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");
            }

            var detailWindow = new KeycapDetailView
            {
                DataContext = new KeycapDetailViewModel(selectedKeycap, _keycapServices)
            };

            detailWindow.ShowDialog();

            _ = LoadKeycaps();
        }

        public async Task LoadKeycaps()
        {
            var keycap = await _keycapServices.GetListAll();
            Keycaps.Clear();
            foreach (var key in keycap)
            {
                Keycaps.Add(key);
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

            string filePath = Path.Combine(excelFolder, "KeycapsData.xlsx");

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Keycaps");
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Quantity";
                worksheet.Cells[1, 4].Value = "Price";
                worksheet.Cells[1, 5].Value = "Description";
                worksheet.Cells[1, 6].Value = "Discount";
                worksheet.Cells[1, 7].Value = "Date";
                worksheet.Cells[1, 8].Value = "Category";
                worksheet.Cells[1, 9].Value = "Brand";
                worksheet.Cells[1, 10].Value = "Origin";
                worksheet.Cells[1, 11].Value = "Material";
                worksheet.Cells[1, 12].Value = "Layout";
                worksheet.Cells[1, 13].Value = "Thickness";
                worksheet.Cells[1, 14].Value = "Reliability";

                int rowIndex = 2; 
                foreach (var keycap in Keycaps)
                {
                    worksheet.Cells[rowIndex, 1].Value = keycap.Pro.ProId;
                    worksheet.Cells[rowIndex, 2].Value = keycap.Pro.ProName;
                    worksheet.Cells[rowIndex, 3].Value = keycap.Pro.ProQuantity;
                    worksheet.Cells[rowIndex, 4].Value = keycap.Pro.ProPrice;
                    worksheet.Cells[rowIndex, 5].Value = keycap.Pro.ProDescription;
                    worksheet.Cells[rowIndex, 6].Value = keycap.Pro.ProDiscount;
                    worksheet.Cells[rowIndex, 7].Value = keycap.Pro.ProDate.ToString();
                    worksheet.Cells[rowIndex, 8].Value = keycap.Pro.ProCategory;
                    worksheet.Cells[rowIndex, 9].Value = keycap.Pro.ProBrand;
                    worksheet.Cells[rowIndex, 10].Value = keycap.Pro.ProOrigin;
                    worksheet.Cells[rowIndex, 11].Value = keycap.KcMaterial;
                    worksheet.Cells[rowIndex, 12].Value = keycap.KcLayout;
                    worksheet.Cells[rowIndex, 13].Value = keycap.KcThickness;
                    worksheet.Cells[rowIndex, 14].Value = keycap.KcReliability;
                    rowIndex++;
                }
                // Lưu file Excel
                package.SaveAs(new FileInfo(filePath));
            }
            MessageBox.Show($"Dữ liệu đã được xuất ra tại: {filePath}");
        }
    }
}

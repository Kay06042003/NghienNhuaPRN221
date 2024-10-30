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
    public class KitViewModel : BaseViewModel
    {
        private readonly IKitServices _kitService;
        private readonly IProductService _productService;

        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public ObservableCollection<Kit> Kits { get; set; }
        public ICollectionView FilteredKits { get; set; }

        public KitViewModel(IKitServices kitService, IProductService productService)
        {
            Kits = new ObservableCollection<Kit>();
            _kitService = kitService;
            _productService = productService;
            FilteredKits = CollectionViewSource.GetDefaultView(Kits);
            FilteredKits.Filter = FilterKits;
            _ = LoadKits();

            AddNewCommand = new RelayCommand(ShowAddNew);
            ShowDetailCommand = new RelayCommand<Kit>(ShowDetail);
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

        private bool FilterKits(object item)
        {
            if (item is Kit kit)
            {
                return string.IsNullOrEmpty(SearchText) ||
                       kit.Pro.ProName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       kit.Pro.ProPrice.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       kit.Pro.ProBrand.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       kit.Pro.ProOrigin.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       (kit.Pro.ProQuantity?.ToString().Contains(SearchText) ?? false);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredKits.Refresh();
        }

        private async void ShowAddNew(object obj)
        {
            var addKitView = new KitAddView();
            addKitView.DataContext = new KitAddVIewModel(_kitService, _productService);
            addKitView.ShowDialog();
            await LoadKits();
        }

        private async void ShowDetail(Kit selectedKit)
        {
            if (selectedKit == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");
            }

            var detailWindow = new KitDetailView
            {
                DataContext = new KitDetailViewModel(selectedKit, _kitService)
            };

            detailWindow.ShowDialog();

            await LoadKits();
        }

        public async Task LoadKits()
        {
            var kit = await _kitService.GetListAll();
            Kits.Clear();
            foreach (var k in kit)
            {
                Kits.Add(k);
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

            string filePath = Path.Combine(excelFolder, "KitsData.xlsx");

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Kits");

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
                worksheet.Cells[1, 11].Value = "Layout";
                worksheet.Cells[1, 12].Value = "Circuit";
                worksheet.Cells[1, 13].Value = "Mode";
                worksheet.Cells[1, 14].Value = "Case";
                worksheet.Cells[1, 15].Value = "Plate";

                int rowIndex = 2; 
                foreach (var kit in Kits)
                {
                    worksheet.Cells[rowIndex, 1].Value = kit.Pro.ProId;
                    worksheet.Cells[rowIndex, 2].Value = kit.Pro.ProName;
                    worksheet.Cells[rowIndex, 3].Value = kit.Pro.ProQuantity;
                    worksheet.Cells[rowIndex, 4].Value = kit.Pro.ProPrice;
                    worksheet.Cells[rowIndex, 5].Value = kit.Pro.ProDescription;
                    worksheet.Cells[rowIndex, 6].Value = kit.Pro.ProDiscount;
                    worksheet.Cells[rowIndex, 7].Value = kit.Pro.ProDate.ToString();
                    worksheet.Cells[rowIndex, 8].Value = kit.Pro.ProCategory;
                    worksheet.Cells[rowIndex, 9].Value = kit.Pro.ProBrand;
                    worksheet.Cells[rowIndex, 10].Value = kit.Pro.ProOrigin;
                    worksheet.Cells[rowIndex, 11].Value = kit.KitLayout;
                    worksheet.Cells[rowIndex, 12].Value = kit.KitCircuit;
                    worksheet.Cells[rowIndex, 13].Value = kit.KitMode;
                    worksheet.Cells[rowIndex, 14].Value = kit.KitCase;
                    worksheet.Cells[rowIndex, 15].Value = kit.KitPlate;
                    rowIndex++;
                }
                package.SaveAs(new FileInfo(filePath));
            }
            MessageBox.Show($"Dữ liệu đã được xuất ra tại: {filePath}");
        }
    }
}

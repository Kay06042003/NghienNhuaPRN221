using BusinessLogic.Interfaces;
using NghienNhuaWPF.Utilities;
using NghienNhuaWPF.View;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NghienNhuaWPF.ViewModels
{
    public class MouseViewModel : BaseViewModel
    {
        private readonly IMouseServices _mouseServices;
        private readonly IProductService _productService;
        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public ObservableCollection<Models.Mouse> Mouses { get; set; }
        public ICollectionView FilteredMouses { get; set; }

        public MouseViewModel(IMouseServices mouseServices, IProductService productService)
        {
            Mouses = new ObservableCollection<Models.Mouse>();
            _mouseServices = mouseServices;
            _productService = productService;

            FilteredMouses = CollectionViewSource.GetDefaultView(Mouses);
            FilteredMouses.Filter = FilterMouse;

            _ = LoadMouses();

            ShowDetailCommand = new RelayCommand<Models.Mouse>(ShowDetail);
            AddNewCommand = new RelayCommand(ShowAddNew);
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

        private bool FilterMouse(object item)
        {
            if (item is Models.Mouse mouse)
            {
                return string.IsNullOrEmpty(SearchText) ||
                       mouse.Pro.ProName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       mouse.Pro.ProPrice.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       mouse.Pro.ProBrand.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       mouse.Pro.ProOrigin.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       (mouse.Pro.ProQuantity?.ToString().Contains(SearchText) ?? false);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredMouses.Refresh();
        }

        private async void ShowAddNew(object obj)
        {
            var addKeyboardView = new MouseAddView();
            addKeyboardView.DataContext = new MouseAddViewModel(_productService, _mouseServices);
            addKeyboardView.ShowDialog();
            await LoadMouses();
        }

        private async void ShowDetail(Models.Mouse selectedMouse)
        {
            if (selectedMouse == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");
                return;
            }

            var detailWindow = new MouseDetailView
            {
                DataContext = new MouseDetailViewModel(selectedMouse, _mouseServices)
            };

            detailWindow.ShowDialog();
            await LoadMouses();
        }

        public async Task LoadMouses()
        {
            var mouse = await _mouseServices.GetListAll();
            Mouses.Clear();
            foreach (var m in mouse)
            {
                Mouses.Add(m);
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

            string filePath = Path.Combine(excelFolder, "MousesData.xlsx");

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Mouses");

                    string[] headers = { "ID", "Name", "Quantity", "Price", "Description", "Discount", "Date", "Category", "Brand", "Origin", "Dpi", "Wire Length", "Led", "Type Battery", "Weight", "Compatibility" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int rowIndex = 2;
                    foreach (var mouse in Mouses)
                    {
                        worksheet.Cells[rowIndex, 1].Value = mouse.Pro.ProId;
                        worksheet.Cells[rowIndex, 2].Value = mouse.Pro.ProName;
                        worksheet.Cells[rowIndex, 3].Value = mouse.Pro.ProQuantity;
                        worksheet.Cells[rowIndex, 4].Value = mouse.Pro.ProPrice;
                        worksheet.Cells[rowIndex, 5].Value = mouse.Pro.ProDescription;
                        worksheet.Cells[rowIndex, 6].Value = mouse.Pro.ProDiscount;
                        worksheet.Cells[rowIndex, 7].Value = mouse.Pro.ProDate.ToString();
                        worksheet.Cells[rowIndex, 8].Value = mouse.Pro.ProCategory;
                        worksheet.Cells[rowIndex, 9].Value = mouse.Pro.ProBrand;
                        worksheet.Cells[rowIndex, 10].Value = mouse.Pro.ProOrigin;
                        worksheet.Cells[rowIndex, 11].Value = mouse.MouseDpi;
                        worksheet.Cells[rowIndex, 12].Value = mouse.MouseWireLength;
                        worksheet.Cells[rowIndex, 13].Value = mouse.MouseLed;
                        worksheet.Cells[rowIndex, 14].Value = mouse.MouseTypeBattery;
                        worksheet.Cells[rowIndex, 15].Value = mouse.MouseWeight;
                        worksheet.Cells[rowIndex, 16].Value = mouse.MouseCompatibility;

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

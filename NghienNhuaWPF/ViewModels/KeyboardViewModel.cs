using BusinessLogic.Interfaces;
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
    public class KeyboardViewModel : BaseViewModel
    {
        private readonly IKeyboardServices _keyboardServices;
        private readonly IProductService _productService;
        public ICommand ShowDetailCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ObservableCollection<KeyBoard> KeyBoards { get; set; }
        public ICollectionView FilteredKeyboards { get; set; }


        public KeyboardViewModel(IKeyboardServices keyboardServices, IProductService productService)
        {
            KeyBoards = new ObservableCollection<KeyBoard>();
            _keyboardServices = keyboardServices;
            _productService = productService;

            FilteredKeyboards = CollectionViewSource.GetDefaultView(KeyBoards);
            FilteredKeyboards.Filter = FilterKeyboards;

            _ = LoadKeyboards();

            ShowDetailCommand = new RelayCommand<KeyBoard>(ShowDetail);
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

        private bool FilterKeyboards(object item)
        {
            if (item is KeyBoard keyboard)
            {
                // Điều chỉnh bộ lọc để tìm kiếm theo các trường bạn cần
                return string.IsNullOrEmpty(SearchText) ||
                       keyboard.Pro.ProName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       keyboard.Pro.ProPrice.Contains(SearchText, StringComparison.OrdinalIgnoreCase)||
                       keyboard.Pro.ProBrand.Contains(SearchText, StringComparison.OrdinalIgnoreCase)||
                       keyboard.Pro.ProOrigin.Contains(SearchText, StringComparison.OrdinalIgnoreCase)||
                       (keyboard.Pro.ProQuantity?.ToString().Contains(SearchText) ?? false);
            }
            return false;
        }

        private void ApplyFilter()
        {
            FilteredKeyboards.Refresh();
        }

        private async void ShowAddNew(object obj)
        {
            var addKeyboardView = new KeyboardAddView();
            addKeyboardView.DataContext = new KeyboardAddViewModel(_productService, _keyboardServices);
            addKeyboardView.ShowDialog();
            await LoadKeyboards();
        }

        private async void ShowDetail(KeyBoard selectedKeyboard)
        {
            if (selectedKeyboard == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn phím để cập nhật hoặc xóa.");
                return;
            }

            var detailWindow = new KeyboardDetail
            {
                DataContext = new KeyboardDetailViewModel(selectedKeyboard, _keyboardServices)
            };
            detailWindow.ShowDialog();
            await LoadKeyboards();
        }

        public async Task LoadKeyboards()
        {
            var keyboard = await _keyboardServices.GetListAll();
            KeyBoards.Clear();
            foreach (var key in keyboard)
            {
                KeyBoards.Add(key);
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

            string filePath = Path.Combine(excelFolder, "KeyboardsData.xlsx");

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Keyboards");

                    string[] headers = { "ID", "Name", "Quantity", "Price", "Description", "Discount", "Date", "Category", "Brand", "Origin", "Led", "Mode", "Switch", "Keycap", "Plate", "Case" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    int rowIndex = 2;
                    foreach (var keyboard in KeyBoards)
                    {
                        worksheet.Cells[rowIndex, 1].Value = keyboard.Pro.ProId;
                        worksheet.Cells[rowIndex, 2].Value = keyboard.Pro.ProName;
                        worksheet.Cells[rowIndex, 3].Value = keyboard.Pro.ProQuantity;
                        worksheet.Cells[rowIndex, 4].Value = keyboard.Pro.ProPrice;
                        worksheet.Cells[rowIndex, 5].Value = keyboard.Pro.ProDescription;
                        worksheet.Cells[rowIndex, 6].Value = keyboard.Pro.ProDiscount;
                        worksheet.Cells[rowIndex, 7].Value = keyboard.Pro.ProDate.ToString();
                        worksheet.Cells[rowIndex, 8].Value = keyboard.Pro.ProCategory;
                        worksheet.Cells[rowIndex, 9].Value = keyboard.Pro.ProBrand;
                        worksheet.Cells[rowIndex, 10].Value = keyboard.Pro.ProOrigin;
                        worksheet.Cells[rowIndex, 11].Value = keyboard.KbLed;
                        worksheet.Cells[rowIndex, 12].Value = keyboard.KbMode;
                        worksheet.Cells[rowIndex, 13].Value = keyboard.KbSwitch;
                        worksheet.Cells[rowIndex, 14].Value = keyboard.KbKeycap;
                        worksheet.Cells[rowIndex, 15].Value = keyboard.KbPlate;
                        worksheet.Cells[rowIndex, 16].Value = keyboard.KbCase;

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

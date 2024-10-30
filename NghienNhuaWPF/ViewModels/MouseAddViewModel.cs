using BusinessLogic.Interfaces;
using Models;
using NghienNhuaWPF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace NghienNhuaWPF.ViewModels
{
    public class MouseAddViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly IMouseServices _mouseServices;

        public ICommand AddMouseCommand { get; }
        public ICommand SelectImagesCommand { get; }
        public ICommand BackToListCommand { get; }

        private List<string> selectedImageFiles = new List<string>();

        public MouseAddViewModel(IProductService productService, IMouseServices mouseServices)
        {
            _productService = productService;
            _mouseServices = mouseServices;

            SelectImagesCommand = new RelayCommand(SelectImages);
            AddMouseCommand = new RelayCommand(AddMouse);
            BackToListCommand = new RelayCommand(BackToList);
        }

        private string _proName;
        public string ProName
        {
            get { return _proName; }
            set
            {
                _proName = value;
                OnPropertyChanged(nameof(ProName));
            }
        }

        private int? _proQuantity;
        public int? ProQuantity
        {
            get { return _proQuantity; }
            set
            {
                _proQuantity = value;
                OnPropertyChanged(nameof(ProQuantity));
            }
        }

        private string _proPrice;
        public string ProPrice
        {
            get { return _proPrice; }
            set
            {
                _proPrice = value;
                OnPropertyChanged(nameof(ProPrice));
            }
        }

        private string _proImage;
        public string ProImage
        {
            get { return _proImage; }
            set
            {
                _proImage = value;
                OnPropertyChanged(nameof(ProImage));
            }
        }

        private string _proDescription;
        public string ProDescription
        {
            get { return _proDescription; }
            set
            {
                _proDescription = value;
                OnPropertyChanged(nameof(ProDescription));
            }
        }

        private string _proDiscount;
        public string ProDiscount
        {
            get { return _proDiscount; }
            set
            {
                _proDiscount = value;
                OnPropertyChanged(nameof(ProDiscount));
            }
        }

        private DateTime? _proDate;
        public DateTime? ProDate
        {
            get { return _proDate; }
            set
            {
                _proDate = value;
                OnPropertyChanged(nameof(ProDate));
            }
        }

        private string _proCategory;
        public string ProCategory
        {
            get { return _proCategory; }
            set
            {
                _proCategory = value;
                OnPropertyChanged(nameof(ProCategory));
            }
        }

        private string _proBrand;
        public string ProBrand
        {
            get { return _proBrand; }
            set
            {
                _proBrand = value;
                OnPropertyChanged(nameof(ProBrand));
            }
        }

        private string _proOrigin;
        public string ProOrigin
        {
            get { return _proOrigin; }
            set
            {
                _proOrigin = value;
                OnPropertyChanged(nameof(ProOrigin));
            }
        }

        private string _mouseDpi;
        public string MouseDpi
        {
            get { return _mouseDpi; }
            set
            {
                _mouseDpi = value;
                OnPropertyChanged(nameof(MouseDpi));
            }
        }

        private string _mouseWireLength;
        public string MouseWireLength
        {
            get { return _mouseWireLength; }
            set
            {
                _mouseWireLength = value;
                OnPropertyChanged(nameof(MouseWireLength));
            }
        }

        private string _mouseLed;
        public string MouseLed
        {
            get { return _mouseLed; }
            set
            {
                _mouseLed = value;
                OnPropertyChanged(nameof(MouseLed));
            }
        }

        private string _mouseTypeBattery;
        public string MouseTypeBattery
        {
            get { return _mouseTypeBattery; }
            set
            {
                _mouseTypeBattery = value;
                OnPropertyChanged(nameof(MouseTypeBattery));
            }
        }

        private string _mouseWeight;
        public string MouseWeight
        {
            get { return _mouseWeight; }
            set
            {
                _mouseWeight = value;
                OnPropertyChanged(nameof(MouseWeight));
            }
        }

        private string _mouseCompatibility;
        public string MouseCompatibility
        {
            get { return _mouseCompatibility; }
            set
            {
                _mouseCompatibility = value;
                OnPropertyChanged(nameof(MouseCompatibility));
            }
        }

        private void SelectImages(object obj)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                selectedImageFiles = openFileDialog.FileNames.ToList();
                ProImage = string.Join("&", selectedImageFiles.Select(Path.GetFileName));
            }
        }

        private async void AddMouse(object obj)
        {
            if (!ValidateInput())
            {
                return;
            }
            bool imagesProcessed = true; 

            try
            {
                if (selectedImageFiles != null && selectedImageFiles.Count > 0)
                {
                    var savedImages = SaveImagesToFolder(selectedImageFiles, ref imagesProcessed);
                    if (savedImages != null && savedImages.Count > 0)
                    {
                        ProImage = string.Join("&", savedImages);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn ảnh nào. Vui lòng chọn ít nhất một ảnh.");
                    return;
                }
                if (!imagesProcessed)
                {
                    MessageBox.Show("Thêm bị hủy do không ghi đè ảnh.");
                    return;
                }
                var product = new Product
                {
                    ProName = this.ProName,
                    ProQuantity = this.ProQuantity,
                    ProPrice = this.ProPrice,
                    ProImage = this.ProImage, 
                    ProDescription = this.ProDescription,
                    ProDiscount = this.ProDiscount,
                    ProDate = DateTime.Now,
                    ProCategory = "Mouse",
                    ProBrand = this.ProBrand,
                    ProOrigin = this.ProOrigin,
                };

                await _productService.AddProduct(product);
                int createdProId = product.ProId;

                var mouse = new Models.Mouse
                {
                    MouseDpi =  this.MouseDpi,
                    MouseWireLength = this.MouseWireLength,
                    MouseLed = this.MouseLed,
                    MouseTypeBattery = this.MouseTypeBattery,
                    MouseWeight = this.MouseWeight,
                    MouseCompatibility = this.MouseCompatibility,
                    ProId = createdProId,
                };

                await _mouseServices.Add(mouse);
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm sản phẩm.");
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }

        private List<string> SaveImagesToFolder(List<string> imageFiles, ref bool imagesProcessed)
        {
            List<string> savedImages = new List<string>();
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string folderPath = Path.Combine(projectRoot, "Images");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (string file in imageFiles)
            {
                string destFileName = Path.Combine(folderPath, Path.GetFileName(file));

                if (File.Exists(destFileName))
                {
                    var result = MessageBox.Show($"Ảnh '{Path.GetFileName(file)}' đã tồn tại. Bạn có muốn ghi đè không?",
                                                   "Thông báo",
                                                   MessageBoxButton.YesNo,
                                                   MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        imagesProcessed = false;
                        continue;
                    }
                }
                if (imagesProcessed)
                {
                    File.Copy(file, destFileName, true);
                    savedImages.Add(Path.GetFileName(file));
                }
            }
            return savedImages;
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private void BackToList(object obj)
        {
            var window = Application.Current.Windows.OfType<Window>()
                         .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ProName) ||
                string.IsNullOrWhiteSpace(ProImage) ||
                string.IsNullOrWhiteSpace(ProPrice) ||
                string.IsNullOrWhiteSpace(ProDescription) ||
                string.IsNullOrWhiteSpace(ProBrand) ||
                string.IsNullOrWhiteSpace(ProOrigin) ||
                string.IsNullOrWhiteSpace(MouseDpi) ||
                string.IsNullOrWhiteSpace(MouseWireLength) ||
                string.IsNullOrWhiteSpace(MouseLed) ||
                string.IsNullOrWhiteSpace(MouseTypeBattery) ||
                string.IsNullOrWhiteSpace(MouseWeight) ||
                string.IsNullOrWhiteSpace(MouseCompatibility))
            {
                MessageBox.Show("Please fill all the fields");
                return false;
            }
            if (int.Parse(ProDiscount) < 0 || int.Parse(ProDiscount) > 100)
            {
                MessageBox.Show("Discount must be greater than zero and less than 100");
                return false;
            }
            if (int.Parse(ProPrice) < 0)
            {
                MessageBox.Show("Price cannot be less than zero");
                return false;
            }
            if (ProQuantity < 0)
            {
                MessageBox.Show("Quantity must not be less than zero");
                return false;
            }
            return true;
        }
    }
}
